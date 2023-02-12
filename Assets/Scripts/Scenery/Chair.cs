using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Scenery
{
    public class Chair : MonoBehaviour
    {
        [SerializeField] Transform sitTransform;
        [SerializeField] string animationTrigger;



        public Transform SitTransform { get { return sitTransform; } }
        public string AnimationTrigger { get { return animationTrigger; } }
 
    }

}


