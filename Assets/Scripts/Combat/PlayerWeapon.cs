using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class PlayerWeapon : Weapon
    {
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] AudioClip firingSound;
        [SerializeField] GameObject hitVFX;


        bool canShoot = true;


        Camera FPCamera;
        WeaponConfig weaponConfig;
        GameObject player;
        AmmunitionStore playerAmmunitionStore;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerAmmunitionStore = player.GetComponent<AmmunitionStore>();
        }

        private void OnEnable()
        {
            canShoot = true;
        }

        void Update()
        {
            if (Input.GetButtonDown("Fire1") && canShoot)
            {
                StartCoroutine(Shoot());
            }

        }

        public void SetFPCamera(Camera camera)
        {
            FPCamera = camera;
        }

        public void Setup(Camera camera, WeaponConfig config)
        {
            FPCamera = camera;
            weaponConfig = config;
        }

        private IEnumerator Shoot()
        {
            canShoot = false;

            if (playerAmmunitionStore.GetAmmunitionLevel(weaponConfig.AmmunitionType) > 0)
            {
                PlayMuzzleFlash();
                PlayFiringSound();
                ProcessRayCast();
                playerAmmunitionStore.DrcreaseesAmmunitionLevel(weaponConfig.AmmunitionType, 1);
            }
            yield return new WaitForSeconds(weaponConfig.TimeBetweenShots);
            canShoot = true;
        }

        private void PlayFiringSound()
        {
            if (firingSound != null)
            {
                AudioSource.PlayClipAtPoint(firingSound, transform.position);
            }
        }

        private void PlayMuzzleFlash()
        {
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }
        }

        private void ProcessRayCast()
        {
            RaycastHit hit;
            bool raycastForward = Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, weaponConfig.WeaponRange);

            if (raycastForward)
            {
                CreateHitImpact(hit);
                Health target = hit.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(weaponConfig.WeaponDamage);
                }

            }
        }

        private void CreateHitImpact(RaycastHit hit)
        {
            if (hitVFX == null) return;
            GameObject hitEffect = Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(hitEffect, 0.5f);
        }


    }

}


