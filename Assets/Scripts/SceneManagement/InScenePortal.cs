using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.Control;
using RPG.Core;


namespace RPG.SceneManagement
{
    public class InScenePortal : MonoBehaviour
    {
        [SerializeField] float fadeTime = 1f;

        [SerializeField] Transform spawnPoint;
        [SerializeField] InScenePortal destinationPortal;
        [SerializeField] bool playerUsablePortal = true;

        private void OnTriggerEnter(Collider other)
        {
           if (other.tag == "Player")
            {
                return;
            }
           else
            {
                UpdatePortalActivator(other.gameObject);
            }
        }



        private void UpdatePortalActivator(GameObject portalActivator)
        {
            if(portalActivator.GetComponent<NavMeshAgent>() == null) return;
            portalActivator.GetComponent<NavMeshAgent>().Warp(destinationPortal.spawnPoint.position);
            portalActivator.transform.rotation = destinationPortal.spawnPoint.rotation;
        }




        void OnDrawGizmosSelected()
        {
            try
            {
               SphereCollider collider = GetComponent<SphereCollider>();
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, collider.radius);
                Gizmos.DrawWireSphere(spawnPoint.position, 0.1f);

            }
            catch (Exception ex)
            {
                Debug.LogError("unable tp draw portal gizmo " + ex.Message);
            }
        }


    }


}

