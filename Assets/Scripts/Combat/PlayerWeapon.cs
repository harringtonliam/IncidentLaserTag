using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Combat
{
    public class PlayerWeapon : MonoBehaviour
    {
        [SerializeField] GameObject laserVFX;
        [SerializeField] AudioSource audioSource;
        [SerializeField] int weaponDamage = 1;
        [SerializeField] float weaponFiringPeriod = 0.1f;
        [SerializeField] int weaponAmmo = 1000;
        [SerializeField] AudioClip weaponSFX;


        Coroutine weaponFireCoroutine;

        public int WeaponDamage { get { return weaponDamage; } }


        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                SetLaserActive(true);
                weaponFireCoroutine = StartCoroutine(FireWeaponContinuously());
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                SetLaserActive(false);
                StopCoroutine(weaponFireCoroutine);

            }
        }


        private void SetLaserActive(bool activate)
        {
            var emissionModule = laserVFX.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = activate;
        }

        IEnumerator FireWeaponContinuously()
        {
            while (true)
            {
                if (weaponAmmo > 0)
                {
                    weaponAmmo--;

                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(weaponSFX);
                    }
                }
                else
                {
                    SetLaserActive(false);
                }
                yield return new WaitForSeconds(weaponFiringPeriod);

            }

        }


    }

}


