using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] GameObject laserVFX;
        [SerializeField] int weaponDamage = 1;

        public int WeaponDamage { get { return weaponDamage; } }


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                SetLaserActive(true);
            }
            else
            {
                SetLaserActive(false);
            }
        }


        private void SetLaserActive(bool activate)
        {
            var emissionModule = laserVFX.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = activate;
        }


    }

}


