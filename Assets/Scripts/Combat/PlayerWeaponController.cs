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

        void Update()
        {
            ProcessKeyInput();
            ProcessScrollWheel();
        }


        public Weapon EquipWeapon(WeaponConfig weaponConfig)
        {
            currentWeaponConfig = weaponConfig;
            Animator animator = GetComponent<Animator>();
            currentWeapon = (PlayerWeapon)currentWeaponConfig.Spawn(weaponPositionTransform);
            currentWeapon.Setup(FPCamera, currentWeaponConfig);
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

        private void ProcessKeyInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponStore.SetActiveWeapon(0);
            }
                if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weaponStore.SetActiveWeapon(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                weaponStore.SetActiveWeapon(2);
            }
        }

        private void ProcessScrollWheel()
        {
            int currentWeaponIndex = weaponStore.GetActiveWeaponIndex();
            int previousWeaponIndex = currentWeaponIndex;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (currentWeaponIndex >= weaponStore.GetLength() - 1)
                {
                    currentWeaponIndex = 0;
                }
                else
                {
                    currentWeaponIndex++;
                }
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (currentWeaponIndex <= 0)
                {
                    currentWeaponIndex = weaponStore.GetLength() - 1;
                }
                else
                {
                    currentWeaponIndex--;
                }
            }

            if(previousWeaponIndex != currentWeaponIndex)
            {
                weaponStore.SetActiveWeapon(currentWeaponIndex);
            }
            
        }
    }

}


