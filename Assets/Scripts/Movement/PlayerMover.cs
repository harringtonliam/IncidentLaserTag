using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;


namespace RPG.Movement
{
    public class PlayerMover : MonoBehaviour,  ISaveable
    {
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxPathLength = 40f;
        [SerializeField] AudioSource footStepSound;


        private Rigidbody rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {


            UpdateAnimator();

        }

        private void UpdateAnimator()
        {
            //Global Velocity
            Vector3 velocity = rigidbody.velocity;
            //local character velocity 
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            //forward speed
            float speed = localVelocity.z;

            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }





        //AnimationEvents
        public void FootR()
        {
            PlayFootStepSound();
        }
        public void FootL()
        {
            PlayFootStepSound();
        }

        private void PlayFootStepSound()
        {
            if (footStepSound != null)
            {
                footStepSound.Play();
            }
        }




        public object CaptureState()
        {
            //can also do this usinga struct
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }
        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;

            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();

        }

    }
}
    
