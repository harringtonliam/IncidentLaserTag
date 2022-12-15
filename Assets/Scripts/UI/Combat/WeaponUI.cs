using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Combat;

namespace RPG.UI.Combat
{

    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] WeaponSlotUI weaponSlotUIPrefab;

        GameObject player;
        WeaponStore playerWeaponStore;
        AmmunitionStore playerAmmunitionStore;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerWeaponStore = player.GetComponent<WeaponStore>();
            playerAmmunitionStore = player.GetComponent<AmmunitionStore>();

            playerWeaponStore.storeUpdated += UpdateWeapons;
            playerAmmunitionStore.storeUpdated += UpdateAmmunitionLevels;

        }

        private void Start()
        {
            UpdateWeapons();
            UpdateAmmunitionLevels();
        }


        private void UpdateWeapons()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = playerWeaponStore.GetDockedItems().Length-1; i >-1; i--)
            {
                var dockedItemSlot = playerWeaponStore.GetDockedItems()[i];
                if (dockedItemSlot.number > 0)
                {
                    WeaponSlotUI weaponSlotUI = Instantiate(weaponSlotUIPrefab, transform);
                    int ammunitionAmount = playerAmmunitionStore.GetAmmunitionLevel(dockedItemSlot.weaponConfig.AmmunitionType);
                    weaponSlotUI.Setup(dockedItemSlot.weaponConfig.Icon, dockedItemSlot.weaponConfig.DisplayName, ammunitionAmount, dockedItemSlot.isActive, dockedItemSlot.weaponConfig.AmmunitionType);
                }

            }
        }

        private void UpdateAmmunitionLevels()
        {
            foreach (Transform child in transform)
            {
                WeaponSlotUI weaponSlotUI = child.gameObject.GetComponent<WeaponSlotUI>();
                if (weaponSlotUI == null) return;
                int ammunitionLevel = playerAmmunitionStore.GetAmmunitionLevel(weaponSlotUI.GetAmmunitionType());
                weaponSlotUI.UpdateAmmunitionAmount(ammunitionLevel);
            }
        }

    }
}


