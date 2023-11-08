using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Movement
{
    public class RollAnimation : MonoBehaviour
    {
        CapsuleCollider capsuleCollider;
        float capsuleColliderInitialHeight;

        private void Start()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            capsuleColliderInitialHeight = capsuleCollider.height;
        }


        //Animation Events
        public void StartRolling()
        {
            capsuleCollider.height = capsuleColliderInitialHeight / 2f;
        }

        public void StopRolling()
        {
            capsuleCollider.height = capsuleColliderInitialHeight;
        }
        //Animation Events End
    }

}

