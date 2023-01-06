using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.InventoryControl;

namespace RPG.Scenery
{

    public class Door : MonoBehaviour
    {
        [SerializeField] Transform rightDoor = null;
        [SerializeField] Transform leftDoor = null;
        [SerializeField] Transform rightOpenPosition = null;
        [SerializeField] Transform rightClosedPosition = null;
        [SerializeField] Transform leftOpenPosition = null;
        [SerializeField] Transform leftClosedPosition = null;
        [SerializeField] float doorSpeed = 0.5f;
        [SerializeField] InventoryItem key = null;

        bool opening = false;
        bool closing = false;


        private void Update()
        {
            if (opening)
            {
                OpenDoors();
            }
            else if (closing)
            {
                CloseDoors();
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != "Player") return;
            if (key == null) return;
     

            var playerInventory = other.GetComponent<Inventory>();
            if(playerInventory.HasItem(key))
            {
                StartOpening();
            }
        }

        public void StartOpening()
        {
            closing = false;
            opening = true; 
        }

        public void StartClosing()
        {
            opening = false;
            closing = true;
        }

        private void CloseDoors()
        {
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightClosedPosition.position, doorSpeed * Time.deltaTime);
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftClosedPosition.position, doorSpeed * Time.deltaTime);
            float distanceToClosed = Vector3.Distance(rightDoor.position, rightClosedPosition.position);
            if (Mathf.Approximately(distanceToClosed, 0f))
            {
                closing = false;
            }
        }

        private void OpenDoors()
        {
            rightDoor.position = Vector3.MoveTowards(rightDoor.position, rightOpenPosition.position, doorSpeed * Time.deltaTime);
            leftDoor.position = Vector3.MoveTowards(leftDoor.position, leftOpenPosition.position, doorSpeed * Time.deltaTime);
            float distanceToOpen = Vector3.Distance(rightDoor.position, rightOpenPosition.position);
            if (Mathf.Approximately(distanceToOpen, 0f))
            {
                opening = false;
            }
        }

    }

}


