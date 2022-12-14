using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    public class PlayerWeapon : Weapon
    {


        [SerializeField] float range = 100f;
        [SerializeField] int damage = 10;
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] AudioClip firingSound;
        [SerializeField] GameObject hitVFX;
        //[SerializeField] Ammo ammoSlot;
        //[SerializeField] AmmoType ammoType;
        [SerializeField] float timeBetweenShots = 0.5f;

        bool canShoot = true;


        Camera FPCamera;

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
            yield return new WaitForSeconds(timeBetweenShots);
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
            Debug.Log("ProcessRayCast   ");
            RaycastHit hit;
            bool raycastForward = Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward, out hit, range);

            if (raycastForward)
            {
                CreateHitImpact(hit);
                Health target = hit.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.TakeDamage(damage);
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


