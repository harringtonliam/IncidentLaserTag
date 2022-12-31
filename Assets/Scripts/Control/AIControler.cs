using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using RPG.Attributes;

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
        [SerializeField] float guardPositionTolerance = 0.25f;

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
            guardPosition = transform.position;
            guardPositionRotation = transform.rotation;
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider != null)
            {
                AIControllerHeight = capsuleCollider.height;
            } 
 
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;

            if (GetComponent<Health>().IsDead) return;

            if (InteractWithCombat()) return;
            if (InteractWithSuspicsion()) return;
            if (InteractWithPatrolPath()) return;
            if (InteractWithTransformDestination()) return;
            if (InteractWithGuardPosition()) return;
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

            if (timeAtWaypoint > waypointPauseTime)
            {
                mover.StartMovementAction(GetCurrentWaypoint(), patrolSpeedFraction);
            }

            return true;
        }

        private bool InteractWithTransformDestination()
        {
            if (transformDestination == null) return false;

            mover.StartMovementAction(transformDestination.position, patrolSpeedFraction);
            return true;

        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWayPoint <= waypointTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex, doPatrolPathOnce);
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
            mover.StartMovementAction(guardPosition, patrolSpeedFraction);
            if (AtGuardPosition())
            {
                transform.rotation = guardPositionRotation;
            }
            return true;
        }

        private bool AtGuardPosition()
        {
            float distanceToGuardPosition = Vector3.Distance(transform.position, guardPosition);
            if (distanceToGuardPosition <= guardPositionTolerance)
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



        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
