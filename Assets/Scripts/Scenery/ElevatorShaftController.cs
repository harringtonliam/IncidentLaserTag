using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scenery

{


    public class ElevatorShaftController : MonoBehaviour
    {
        [SerializeField] Transform elevator;
        [SerializeField] Transform[] doors;
        [SerializeField] float elevatorSpeed = 1f;
        [SerializeField] float elevatorPauseTime = 6f;
        [SerializeField] float doorDistanceTolerance = 0.1f;
        [SerializeField] int doorIndex = 0;
        [SerializeField] bool isGoingUp = true;
        float timeAtDoor;




        // Update is called once per frame
        void Update()
        {
            MoveElevator();
        }

        private void MoveElevator()
        {


            timeAtDoor += Time.deltaTime;

            if (AtDoor())
            {
                Debug.Log("Elevator at door");
                timeAtDoor = 0;
                GetNextDoor();
            }




            if (timeAtDoor > elevatorPauseTime)
            {
                elevator.position = Vector3.MoveTowards(elevator.position, GetPositionToMoveTo(), elevatorSpeed * Time.deltaTime);
            }


        }


        private bool AtDoor()
        {
            float distanceToDoor = Vector3.Distance(elevator.position, GetPositionToMoveTo());

            if (distanceToDoor <= doorDistanceTolerance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetNextDoor()
        {
            if (doorIndex == 0 )
            {
                isGoingUp = true;
            }
            if (doorIndex >= doors.Length-1)
            {
                isGoingUp = false;
            }

            if (isGoingUp)
            {
                doorIndex++;
            }
            else
            {
                doorIndex--;
            }
        }

        private Vector3 GetPositionToMoveTo()
        {
            Vector3 positionToMoveTo = new Vector3(elevator.position.x, doors[doorIndex].position.y, elevator.position.z);
            return positionToMoveTo;
        }

    }

}


