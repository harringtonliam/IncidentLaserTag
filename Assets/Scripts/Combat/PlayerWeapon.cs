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
        //[SerializeField] Ammo ammoSlot;
        //[SerializeField] 


        bool canShoot = true;


        Camera FPCamera;
        WeaponConfig weaponConfig;

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
            //TODO: Add ammunition
            //if (ammoSlot.GetAmmoAmount(ammoType) > 0)
            //{
            PlayMuzzleFlash();
            PlayFiringSound();
            ProcessRayCast();
            //ammoSlot.DecreaseAmmoAmount(ammoType);
            //}
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


