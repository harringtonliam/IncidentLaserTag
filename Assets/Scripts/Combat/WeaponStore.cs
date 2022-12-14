using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using System;
using RPG.InventoryControl;

namespace RPG.Combat
{
    public class WeaponStore : MonoBehaviour, ISaveable
    {
        [SerializeField]
        DockedItemSlot[] dockedItems = new  DockedItemSlot[4];

        [Serializable]
        public class DockedItemSlot
        {
            public WeaponConfig weaponConfig;
            public int number;
            public bool isActive;
        }

        public event Action storeUpdated;

        private void Start()
        {
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public int GetLength()
        {
            return dockedItems.Length;
        }

        public WeaponConfig GetAction(int index)
        {
            if (dockedItems.Length <= index)
            {
                return dockedItems[index].weaponConfig;
            }
            return null;
        }

        public int GetNumber(int index)
        {
            if (dockedItems.Length > index)
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        public DockedItemSlot[] GetDockedItems () { return dockedItems; } 



        public void AddAction(InventoryItem item, int index, int number, bool isActive)
        {
            if (object.ReferenceEquals(item, dockedItems[index].weaponConfig))
            {
               dockedItems[index].number += number;
            }
            else
            {
                var slot = new DockedItemSlot();
                slot.weaponConfig = item as WeaponConfig;
                slot.number = number;
                slot.isActive = isActive;
                dockedItems[index] = slot;
            }
            SetActiveWeapon(index);
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

        public void RemoveItems(int index, int number)
        {
            if (dockedItems.Length > index)
            {
                dockedItems[index].number -= number;
                if (dockedItems[index].number <= 0)
                {
                    dockedItems[index].weaponConfig = null;
                    dockedItems[index].number = 0;
                    dockedItems[index].isActive = false;
                }
                if (storeUpdated != null)
                {
                    storeUpdated();
                }
            }
        }


        public bool AddToFirstEmptySlot(WeaponConfig item, int number)
        {
            int weaponSlotIndex = -1;

                for (int i = 0; i < dockedItems.Length; i++)
                {
                    if (dockedItems[i].weaponConfig == null)
                    {
                        Debug.Log("found empty ammo slot");
                    weaponSlotIndex = i;
                        break;
                    }
                }
     
            Debug.Log("Weapon slot index " + weaponSlotIndex);

            if (weaponSlotIndex >= 0 && weaponSlotIndex < dockedItems.Length)
            {
                AddAction(item, weaponSlotIndex, number, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        public WeaponConfig GetActiveWeapon()
        {
            foreach (var dockedItem  in dockedItems)
            {
                if (dockedItem.isActive)
                {
                    return dockedItem.weaponConfig;
                }
            }

            return null;
        }

        public int GetActiveWeaponIndex()
        {
            for (int i = 0; i < dockedItems.Length; i++)
            {
                if (dockedItems[i].isActive)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetActiveWeapon(int slot)
        {

            if (dockedItems[slot] != null)
            {
                foreach (var dockedItem in dockedItems)
                {
                    dockedItem.isActive = false;
                }
                dockedItems[slot].isActive = true;
            }

            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }


        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as WeaponConfig;
            if (!actionItem) return 0;

            if (dockedItems.Length <= index && !object.ReferenceEquals(item, dockedItems[index].weaponConfig))
            {
                return 0;
            }
            if (actionItem.IsStackable)
            {
                return item.MaxNumberInStack;
            }
            if (dockedItems.Length <= index)
            {
                return 0;
            }

            return 1;
        }

        public void FindWeaponToChangeTo()
        {
            AmmunitionStore ammunitionStore = GetComponent<AmmunitionStore>();
            int emptySlot = 0;
            for (int i = 0; i < dockedItems.Length; i++)
            {
                if (dockedItems[i].weaponConfig != null && dockedItems[i].weaponConfig.AmmunitionType == AmmunitionType.None)
                {
                    SetActiveWeapon(i);
                    return;
                }

                if (dockedItems[i].weaponConfig != null  && ammunitionStore.FindAmmunitionType(dockedItems[i].weaponConfig.AmmunitionType) >= 0)
                {
                    SetActiveWeapon(i);
                    return;
                }
                if (dockedItems[i].weaponConfig == null)
                {
                    emptySlot = i;
                }
            }
            SetActiveWeapon(emptySlot);
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
            public bool isActive;
        }

        public object CaptureState()
        {
            var state = new DockedItemRecord[4];
            for (int i = 0; i < dockedItems.Length; i++)
            {
                if (dockedItems[i].weaponConfig != null)
                {
                    state[i].itemID = dockedItems[i].weaponConfig.ItemID;
                    state[i].number = dockedItems[i].number;
                    state[i].isActive = dockedItems[i].isActive;
                }
            }
            return state;
        }
            
        public void RestoreState(object state)
        {
            var stateDict = (DockedItemRecord[])state;
            int activeWeaponIndex = 0;
            for (int i = 0; i < stateDict.Length; i++)
            {
                AddAction(WeaponConfig.GetFromID(stateDict[i].itemID) as WeaponConfig, i, stateDict[i].number, stateDict[i].isActive);
                if (stateDict[i].isActive) activeWeaponIndex = i;
            }
            SetActiveWeapon(activeWeaponIndex);
            if (storeUpdated != null)
            {
                storeUpdated();
            }
        }

    }

}


