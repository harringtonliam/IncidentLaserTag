using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;
using RPG.Scenery;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIControler : MonoBehaviour
    {
        [SerializeField] AIRelationship aIRelationship = AIRelationship.Hostile;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 2f;
        [SerializeField] float aggrevationCoolDownTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] bool doPatrolPathOnce = false;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointPauseTime = 2f;
        [Range(0f, 1f)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;
        [SerializeField] GameObject combatTargetGameObject;
        [SerializeField] Transform transformDestination;
        [SerializeField] Transform guardPositionTransform = null;
        [SerializeField] float guardPositionTolerance = 0.25f;
        [SerializeField] bool isRollEnabled = false;
        [SerializeField] float minRollDistanceFromTarget = 6f;
        [SerializeField] bool isJumpEnabled = false;
        [SerializeField] float jumpDistanceFromTarget = 4f;

        [SerializeField] bool isDebuggingOn = false;

        GameObject player;
        float playerHeight = 0f;
        Mover mover;

        float AIControllerHeight;


        Vector3 guardPosition;
        Quaternion guardPositionRotation;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        float timeAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        bool isWayPointCycled = false;
        ChairController chairController;

        public AIRelationship AIRelationship
        {
            get{ return aIRelationship;}
        }

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            playerHeight  = player.GetComponent<CapsuleCollider>().height;

            mover = GetComponent<Mover>();
            if (aIRelationship == AIRelationship.Hostile)
            {
                combatTargetGameObject = player;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            SetGuardPosition();
            if (patrolPath != null)
            {
                isWayPointCycled = true;
            }

            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                AIControllerHeight = capsuleCollider.height;
            }

            chairController = GetComponent<ChairController>();
        }

        private void SetGuardPosition()
        {
            if (guardPositionTransform == null)
            {
                guardPosition = transform.position;
                guardPositionRotation = transform.rotation;
            }
            else
            {
                guardPosition = guardPositionTransform.position;
                guardPositionRotation = guardPositionTransform.rotation;
            }
        }


        // Update is called once per frame
        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

            if (GetComponent<Health>().IsDead) return;

            if (chairController.IsActionHappening)
            {
                return;
            }

            TriggerAnimations();

            if (InteractWithCombat()) return;
            if (InteractWithSuspicsion()) return;
            if (InteractWithPatrolPath()) return;
            if (InteractWithTransformDestination()) return;
            if (InteractWithGuardPosition()) return;
        }

        private void TriggerAnimations()
        {
            var speed = mover.GetSpeed();
            if (Mathf.Approximately(speed, mover.MaxSpeed) && isRollEnabled  && IsAggrevated()  && DistanceToCombatTarget() > minRollDistanceFromTarget)
            {
                TriggerRoll();
                isRollEnabled = false;
            }
            else if(Mathf.Approximately(speed, mover.MaxSpeed) && isJumpEnabled && IsAggrevated())// && Mathf.Approximately(DistanceToCombatTarget(), jumpDistanceFromTarget))
            {
                TriggerJump();
                isJumpEnabled = false;
            }
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        public void SetChaseDistance(float newChaseDistance)
        {
            chaseDistance = newChaseDistance;
        }

        public void SetPatrolPath(PatrolPath newPatrolPath, bool useNewPatrolPath)
        {
            patrolPath = newPatrolPath;
            doPatrolPathOnce = false;
            currentWaypointIndex = 0;
        }

        public void SetPatrolPath(PatrolPath newPatrolPath, bool useNewPatrolPath, bool doPatrolPathOnce)
        {
            if (patrolPath == newPatrolPath) return;

            patrolPath = newPatrolPath;
            isWayPointCycled = true;
            currentWaypointIndex = 0;
            this.doPatrolPathOnce = doPatrolPathOnce;
        }

        public void SetWayPointPauseTime(float pauseTime)
        {
            waypointPauseTime = pauseTime;
        }

        public void SetPatrolSpeedFraction(float speedFraction)
        {
            patrolSpeedFraction = speedFraction;
        }
        public void SetTransformDestination( Transform destination)
        {
            transformDestination = destination;
        }

        public void SetCombatTarget(GameObject target)
        {
            combatTargetGameObject = target;
        }

        private bool InteractWithPatrolPath()
        {
            if (patrolPath == null) return false;
            
            timeAtWaypoint += Time.deltaTime;

            if (AtWaypoint())
            {
                timeAtWaypoint = 0;
                CycleWaypoint();
               
            }

            if (currentWaypointIndex < 0)
            {
                patrolPath = null;
                return false;
            }

            StandUpFromChairIfNeeded();

            if ((timeAtWaypoint > waypointPauseTime && isWayPointCycled))
            {
                mover.StartMovementAction(GetCurrentWaypoint(), patrolSpeedFraction);
                isWayPointCycled = false;
            }

            return true;
        }

        private bool InteractWithTransformDestination()
        {
            if (transformDestination == null) return false;
            
            if (AtTransformDestination() && IsDestiationAChair(transformDestination) && !chairController.IsInteractingWithChair())
            {
                var chair = transformDestination.GetComponent<Chair>();
                mover.Cancel();
                chairController.SitOnChair(chair);
                return true;
            }
            else if(AtTransformDestination())
            {
                transform.rotation = transformDestination.rotation;
                return true;
            }

            StandUpFromChairIfNeeded();
            mover.StartMovementAction(transformDestination.position, patrolSpeedFraction);
            return true;
        }


        private bool AtWaypoint()
        {
            return  AtPosition(GetCurrentWaypoint(), waypointTolerance);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex, doPatrolPathOnce);
            isWayPointCycled = true;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        
        private bool InteractWithSuspicsion()
        {
            if (timeSinceLastSawPlayer < suspicionTime )
            {
                ActionScheduler actionSchduler = GetComponent<ActionScheduler>();
                actionSchduler.CancelCurrentAction();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool InteractWithGuardPosition()
        {
            if (AtGuardPosition() && IsDestiationAChair(guardPositionTransform) && !chairController.IsInteractingWithChair())
            {
                var chair = guardPositionTransform.GetComponent<Chair>();
                mover.Cancel();
                chairController.SitOnChair(chair);
                return true;
            }
            else if(AtGuardPosition())
            {
                transform.rotation = guardPositionRotation;
                return true;
            }

            StandUpFromChairIfNeeded();

            mover.StartMovementAction(guardPosition, patrolSpeedFraction);
            return true;
        }

        private void StandUpFromChairIfNeeded()
        {
            if (chairController.IsSeated)
            {
                chairController.StandUpFromChair();
            }
        }

        private bool AtGuardPosition()
        {
            float toleranceToUse = guardPositionTolerance;
            if (IsDestiationAChair(guardPositionTransform))
            {
                toleranceToUse = chairController.ChairPositionTolerance;
            }
            return AtPosition(guardPosition, toleranceToUse);
        }

        private bool AtTransformDestination()
        {
            float toleranceToUse = guardPositionTolerance;
            if (IsDestiationAChair(transformDestination))
            {
                toleranceToUse = chairController.ChairPositionTolerance;
            }
            return AtPosition(transformDestination.position, toleranceToUse);
        }

        private bool AtPosition(Vector3 positionToCheck, float tolerance)
        {
            float distanceToDestination = Vector3.Distance(transform.position, positionToCheck);

            if (distanceToDestination <= tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool IsDestiationAChair(Transform destination)
        {
            if (destination == null) return false;
            
            if (destination.GetComponent<Chair>() != null)
            {
                return true;
            }
            if (destination.parent != null && destination.parent.GetComponent<Chair>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        private bool InteractWithCombat()
        {

            Fighting fighter = GetComponent<Fighting>();
            if (combatTargetGameObject == null)
            {
                fighter.Cancel();
                return false;
            }
            if (IsAggrevated() && fighter.CanAttack(combatTargetGameObject))
            {
                DebugMessage("Interact with combat can attack and is agrrivated");
                StandUpFromChairIfNeeded();
                timeSinceLastSawPlayer = 0;
                fighter.Attack(combatTargetGameObject);
                AggrevateNearbyEnemies();
                return true;
            }
            else
            {
                fighter.Cancel();
                return false;
            }
        }

        private void AggrevateNearbyEnemies()
        {
            if (aIRelationship != AIRelationship.Hostile) return;

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0f);
            foreach (var hit in hits)
            {

                AIControler ai = hit.transform.GetComponent<AIControler>();
                if (ai != null && ai != this)
                {
                    ai.Aggrevate();
                }
            }

        }

        private bool IsAggrevated()
        {
            if (timeSinceAggrevated < aggrevationCoolDownTime)
            {
                //aggrevated
                return true;
            }
            if (DistanceToCombatTarget() <= chaseDistance && CheckLineOfSightToCombatTargetOk())
            {
                return true;
            }
            return false;
        }

        private float DistanceToCombatTarget()
        {
            if (combatTargetGameObject == null) return Mathf.Infinity;
            float distance = Vector3.Distance(combatTargetGameObject.transform.position, transform.position);
            return distance;
        }

        private bool CheckLineOfSightToCombatTargetOk()
        {
            bool lineOfSightOk = false;

            Vector3 playerMidPoint = player.transform.position + new Vector3(0f, (playerHeight / 2), 0f);
            Vector3 aiHeadHight = transform.position + new Vector3(0f, (AIControllerHeight), 0f);

            var directionToPlayer = playerMidPoint - aiHeadHight;
            RaycastHit hit;
            bool raycastForward = Physics.Raycast(aiHeadHight, directionToPlayer, out hit, chaseDistance);
            //Debug.DrawLine(aiHeadHight, hit.point, Color.red);
            if (raycastForward && hit.transform.tag == player.tag)
            {
                lineOfSightOk = true;
            }

            return lineOfSightOk;
        }

        public void TriggerRoll()
        {
            GetComponent<Animator>().SetTrigger("roll");
        }

        public void TriggerJump()
        {
            DebugMessage("JUmp Triggered");
            GetComponent<Animator>().SetTrigger("jump");
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        private void DebugMessage(string message)
        {
            if (isDebuggingOn)
            {
                Debug.Log(message + " " + gameObject.name);
            }
        }
    }
}
