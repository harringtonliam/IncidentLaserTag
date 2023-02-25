using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Scenery;
using UnityEngine.AI;

namespace RPG.Control
{
    public class ChairController : MonoBehaviour
    {
        [SerializeField] float chairPositionTolerance = 0.9f;
        [SerializeField] float seatedColiderHeightProportion = 0.75f;


        bool isSeated = false;
        bool isActionHappening = false;

        Vector3 standPosition;
        Chair currentChair;
        ColliderController colliderController;


        public bool IsSeated { get { return isSeated; } }
        public bool IsActionHappening { get { return isActionHappening; } }
        public float ChairPositionTolerance { get { return chairPositionTolerance; } }


        private void Start()
        {
            colliderController = GetComponent<ColliderController>();
        }

        public bool IsInteractingWithChair()
        {
            return isSeated || isActionHappening;
        }


        public void SitOnChair(Chair targetChair)
        {
            if (isActionHappening) return;
            if (targetChair.IsOccupied) return;

            isActionHappening = true;
            GetComponent<NavMeshAgent>().enabled = false;
            standPosition = transform.position;
            transform.position = targetChair.SitTransform.position;
            transform.rotation = targetChair.SitTransform.rotation;
            //Debug.Log("Triggering sit Animation");
            GetComponent<Animator>().SetTrigger("sit");
            colliderController.ResizeCollider(seatedColiderHeightProportion);
            isSeated = true;
            isActionHappening = false;
            currentChair = targetChair;
            currentChair.MakeChairOccuiped(true);
            StartCoroutine(TriggerSeatedAnimation());
        }

        public void StandUpFromChair()
        {
            if (isActionHappening) return;

            isActionHappening = true;
            //Debug.Log("Triggering stand Animation");
            GetComponent<Animator>().SetTrigger("stand");
            colliderController.ResetCollider();
            transform.position = standPosition;
            isSeated = false;
            currentChair.MakeChairOccuiped(false);
            GetComponent<NavMeshAgent>().enabled = true;
            isActionHappening = false;
        }

        private IEnumerator TriggerSeatedAnimation()
        {
            
            yield return new WaitForSeconds(1f);
            if (currentChair.AnimationTrigger != string.Empty && currentChair.AnimationTrigger != "")
            {
                //Debug.Log("Triggering " + currentChair.AnimationTrigger + " Animation");
                //GetComponent<Animator>().SetTrigger("sit");//Make certain the character is sitting, sometimes after being shot this was not the case;
                GetComponent<Animator>().SetTrigger(currentChair.AnimationTrigger);
            }
        }
    }

}


