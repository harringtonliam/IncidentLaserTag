using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Scenery;
using UnityEngine.AI;

namespace RPG.Control
{
    public class ChairController : MonoBehaviour
    {


        bool isSeated = false;
        bool isActionHappening = false;

        Vector3 standPosition;


        public bool IsSeated { get { return isSeated; } }
        public bool IsActionHappening { get { return isActionHappening; } }

        public bool IsInteractingWithChair()
        {
            return isSeated || isActionHappening;
        }


        public void SitOnChair(Chair targetChair)
        {
            Debug.Log("start Sit on Chair");
            if (isActionHappening) return;

            isActionHappening = true;
            GetComponent<NavMeshAgent>().enabled = false;
            standPosition = transform.position;
            transform.position = targetChair.SitTransform.position;
            Debug.Log("Set sit position");
            transform.rotation = targetChair.SitTransform.rotation;
            GetComponent<Animator>().SetTrigger("sit");
            isSeated = true;
            isActionHappening = false;
            Debug.Log("End Sit on Chair");
        }

        public void StandUpFromChair()
        {
            Debug.Log("start Stand up from Chair");

            if (isActionHappening) return;

            isActionHappening = true;
            GetComponent<Animator>().SetTrigger("stand");
            transform.position = standPosition;
            Debug.Log("Set stand position");
            isSeated = false;
            GetComponent<NavMeshAgent>().enabled = true;
            isActionHappening = false;

            Debug.Log("End Stand up from Chair");

        }
    }

}


