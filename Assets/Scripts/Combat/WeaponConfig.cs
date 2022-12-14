using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.InventoryControl;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : EquipableItem
    {
        [SerializeField] AnimatorOverrideController weaponOverrideController = null;
        [SerializeField] Weapon equipedPrefab = null;
        [SerializeField] AmmunitionType ammunitionType = AmmunitionType.None;
        [SerializeField] int weaponDamage = 4;
        [SerializeField] int weaponDamageAdditiveBonus = 0;
        [SerializeField] int weaponToHitBonus = 0;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] bool isRangedWeapon = false;
        [SerializeField] float timeBetweenShots = 0.5f;



        const string weaponName = "Weapon";

        public AmmunitionType AmmunitionType {  get { return ammunitionType; } }

        public float WeaponDamage
        {
            get { return weaponDamage; }
        }


        public float PercentageBonus
        {
            get { return percentageBonus; }
        }

        public float WeaponRange
        {
            get { return weaponRange; }
        }


        public int WeaponToHitBonus
        {
            get { return weaponToHitBonus; }
        }

        public int WeaponDamageAdditiveBonus
        {
            get { return weaponDamageAdditiveBonus; }
        }

        public float TimeBetweenShots
        {
            get { return timeBetweenShots; }
        }


        public bool IsRangedWeapon
        {
            get { return isRangedWeapon; }
        }

        public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            Weapon weapon = null;

            if (equipedPrefab != null)
            {
                Transform handTransform = GetTransform(rightHand, leftHand);
                weapon = Instantiate(equipedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            if (weaponOverrideController != null)
            {
                animator.runtimeAnimatorController = weaponOverrideController;
            }


            return weapon;
        }

        public Weapon Spawn(Transform weaponPosition)
        {
            DestroyOldWeapon(weaponPosition, weaponPosition);

            Weapon weapon = null;

            if (equipedPrefab != null)
            {

                weapon = Instantiate(equipedPrefab, weaponPosition);
                weapon.gameObject.name = weaponName;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if (oldWeapon ==null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null)
            {
                return;
            }
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        public Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }



        public int CalcWeaponDamage()
        {

            return weaponDamage;
        }
            
    }


}
