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

        bool isSeated = false;
        bool isActionHappening = false;

        Vector3 standPosition;
        Chair currentChair;


        public bool IsSeated { get { return isSeated; } }
        public bool IsActionHappening { get { return isActionHappening; } }
        public float ChairPositionTolerance { get { return chairPositionTolerance; } }

        public bool IsInteractingWithChair()
        {
            return isSeated || isActionHappening;
        }


        public void SitOnChair(Chair targetChair)
        {
            if (isActionHappening) return;

            isActionHappening = true;
            GetComponent<NavMeshAgent>().enabled = false;
            standPosition = transform.position;
            transform.position = targetChair.SitTransform.position;
            transform.rotation = targetChair.SitTransform.rotation;
            GetComponent<Animator>().SetTrigger("sit");
            isSeated = true;
            isActionHappening = false;
            currentChair = targetChair;
            StartCoroutine(TriggerSeatedAnimation());
        }

        public void StandUpFromChair()
        {
            if (isActionHappening) return;

            isActionHappening = true;
            GetComponent<Animator>().SetTrigger("stand");
            transform.position = standPosition;
            isSeated = false;
            GetComponent<NavMeshAgent>().enabled = true;
            isActionHappening = false;
        }

        private IEnumerator TriggerSeatedAnimation()
        {
            yield return new WaitForSeconds( 1f);
            if (currentChair.AnimationTrigger != string.Empty && currentChair.AnimationTrigger != "")
            {
                GetComponent<Animator>().SetTrigger(currentChair.AnimationTrigger);
            }
        }
    }

}


