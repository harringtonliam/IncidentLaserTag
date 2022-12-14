using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] Transform weaponPositionTransform = null;
        [SerializeField] Camera FPCamera;


        PlayerWeapon currentWeapon;
        WeaponStore weaponStore;
        WeaponConfig currentWeaponConfig;



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
                EquipWeapon(weaponStore.GetActiveWeapon());
            }
        }


        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Animator animator = GetComponent<Animator>();
            currentWeapon = (PlayerWeapon)currentWeaponConfig.Spawn(weaponPositionTransform);
            currentWeapon.SetFPCamera(FPCamera);
            return currentWeapon;
        }

        private void UpdateWeapon()
        {
            var weaponConfig = weaponStore.GetActiveWeapon() as WeaponConfig;
            if (weaponConfig != null)
            {
                EquipWeapon(weaponConfig);
            }

        }
    }

}


