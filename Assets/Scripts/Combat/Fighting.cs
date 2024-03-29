using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;
using RPG.Saving;
using RPG.Attributes;
using RPG.InventoryControl;

namespace RPG.Combat
{
    public class Fighting : MonoBehaviour, IAction, ISaveable
    {


        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] WeaponConfig defaultWeaponConfig = null;
        [SerializeField] Health target;
        [SerializeField] bool isDebuggingOn = false;

        float timeSinceLastAttack = Mathf.Infinity;
        WeaponConfig currentWeaponConfig;
        Weapon currentWeapon;
        WeaponStore weaponStore;
        AmmunitionStore ammoStore;
        Mover mover;
        Health gameObjectHealth;

        private void Awake()
        {
            weaponStore = GetComponent<WeaponStore>();

            if (weaponStore)
            {
                weaponStore.storeUpdated += UpdateWeapon;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (currentWeaponConfig == null)
            {
               currentWeapon = EquipWeapon(defaultWeaponConfig);
            }

            ammoStore = GetComponent<AmmunitionStore>();

            mover = GetComponent<Mover>();
            gameObjectHealth = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (gameObjectHealth.IsDead)
            {
                mover.Cancel();
                return;
            }

            if (target != null)
            {
                if (target.IsDead)
                {
                    return;
                }

                if(mover != null) mover.MoveTo(target.transform.position, 1f, "fighting");
                if (GetIsInRange(target.transform))
                {
                    if(mover != null) mover.Cancel();
                    AttackBehaviour();
                }
            }
        }

        public Health GetTarget()
        {
            return target;
        }


        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Animator animator = GetComponent<Animator>();
            currentWeapon = currentWeaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
            return currentWeapon;
        }

        private void UpdateWeapon()
        {
            var weaponConfig = weaponStore.GetActiveWeapon() as WeaponConfig;
            if (weaponConfig != null)
            {
                EquipWeapon(weaponConfig);
            }
            else
            {
                EquipWeapon(defaultWeaponConfig);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }

        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Events
        public void Hit()
        {
            if (target == null) return;

            if (!UseAmmunition())
            {
                ChangeWeapon();
                return;
            }
            
            if (currentWeapon != null)
            {
                currentWeapon.OnHit();
            }

            if (currentWeaponConfig.IsRangedWeapon && !CheckLineOfSight())
            {
                return;
            }

            float calculatedDamage = 0;
            calculatedDamage = currentWeaponConfig.CalcWeaponDamage(); 
            target.TakeDamage(calculatedDamage);
  
        }

        private void ChangeWeapon()
        {
            //find a different weapon
            weaponStore.FindWeaponToChangeTo();
        }

        private bool UseAmmunition()
        {
            if (currentWeaponConfig.AmmunitionType == AmmunitionType.None)
            {
                return true;
            }

            AmmunitionStore ammoStore = GetComponent<AmmunitionStore>();

            //search for ammo type in ammostoreslot
            int ammoStoreSlot = ammoStore.FindAmmunitionType(currentWeaponConfig.AmmunitionType);

            //if none found return
            if (ammoStoreSlot < 0)
            {

                return false;
            }

            //reduce ammo in slot by 1
            ammoStore.RemoveItems(ammoStoreSlot, 1);
            return true;

        }

        private bool CheckLineOfSight()
        {
            if (target == null) return false;

            Vector3 lineOfSightStart = currentWeaponConfig.GetTransform(rightHandTransform, leftHandTransform).position;
            lineOfSightStart.x = lineOfSightStart.x + 0.15f;
            Vector3 lineOfSightEnd = GetAimLocation();

            Vector3 lineOfSiteDirection = (lineOfSightEnd - lineOfSightStart).normalized;
    
            Ray ray = new Ray(lineOfSightStart, lineOfSiteDirection);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, currentWeaponConfig.WeaponRange);
            Debug.Log("Check line of site for " + gameObject.name + " target=" + target.gameObject.name +" hit=" + hit.transform.gameObject.name);
            Health targetHealth = hit.transform.GetComponent<Health>();

            if (targetHealth == null || targetHealth != target)
            {
                return false;
            }
           
            return true;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider capsuleCollider = target.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                return target.transform.position;
            }
            return target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

 
        public void Shoot()
        {
            Hit();
        }
        //End of animation events

        private bool GetIsInRange(Transform targetTransform)
        {
            return currentWeaponConfig.WeaponRange >= Vector3.Distance(targetTransform.position, transform.position);
        }


        public void Attack(GameObject combatTarget)
        {
            DebugMessage("Fighting Attack " + combatTarget.name);
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>(); ;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            if (mover != null)
            {
                if (!mover.CanMoveTo(combatTarget.transform.position)
                        && !GetIsInRange(combatTarget.transform))
                {

                    return false;
                }
            }

            if (mover == null)
            {
                if (!GetIsInRange(combatTarget.transform))
                {
                    return false;
                }
            }


            Health targetHealth = combatTarget.GetComponent<Health>();
            if (targetHealth.IsDead)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            if (mover == null)
            {
                mover.Cancel();
            }
           
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public object CaptureState()
        {
            if (currentWeaponConfig!= null)
            {
                return currentWeaponConfig.name;
            }
            else
            {
                return null;
            }
    
        }

        public void RestoreState(object state)
        {
            if (state != null)
            {
                string weaponName = (string)state;
                WeaponConfig weapon = Resources.Load<WeaponConfig>(weaponName);
                EquipWeapon(weapon);
            }

        }

        private void DebugMessage(string message)
        {
            if (isDebuggingOn)
            {
                Debug.Log(message + " " + gameObject.name);
            }
        }

    }
}
