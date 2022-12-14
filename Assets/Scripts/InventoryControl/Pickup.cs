using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Combat;
using RPG.Attributes;
using System;

namespace RPG.InventoryControl
{

    public class Pickup : MonoBehaviour
    {
        //Config
        [SerializeField] InventoryItem inventoryItem = null;
        [SerializeField] int numberOfItems = 1;
        [SerializeField] AudioSource pickupAudioSource;

        //Cached references
        Inventory inventory;

        public InventoryItem InventoryItem { get { return inventoryItem; } }
        public int NumberOfItems { get { return numberOfItems; } }

        GameObject player;

        bool hasbeenPickupUp = false;


        void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }

        public void Setup(InventoryItem item, int number)
        {
            this.inventoryItem = item;
            if (!item.IsStackable)
            {
                number = 1;
            }

            this.numberOfItems = number;
        }

        public void PickupItem()
        {
            if (hasbeenPickupUp) return;
            bool slotFoundOk = true;
            Ammunition ammunitionItem;
            WeaponConfig weaponConfigItem;
            if (inventoryItem.GetType() == typeof(Ammunition))
            {
                ammunitionItem = (Ammunition)inventoryItem;
                slotFoundOk = player.GetComponent<AmmunitionStore>().AddToFirstEmptySlot(ammunitionItem, numberOfItems);
            }
            else if (inventoryItem.GetType() == typeof(WeaponConfig))
            {
                weaponConfigItem = (WeaponConfig)inventoryItem;
                slotFoundOk = player.GetComponent<WeaponStore>().AddToFirstEmptySlot(weaponConfigItem, numberOfItems);
            }
            else if(inventoryItem.GetType() == typeof(MedicalPack))
            {
                MedicalPack medicalPack = (MedicalPack)inventoryItem;
                player.GetComponent<Health>().Heal(medicalPack.MedicalPackHealing);
                slotFoundOk = true;
            }
            else
            { 
                slotFoundOk = inventory.AddToFirstEmptySlot(inventoryItem, numberOfItems);
            }

            if (slotFoundOk)
            {
                hasbeenPickupUp = true;
                PlayPickUpSound();
                Destroy(gameObject, 1f);
                ScenePickups scenePickups = FindObjectOfType<ScenePickups>();
                if (scenePickups != null)
                {
                    scenePickups.RemoveItem(this.inventoryItem, this.numberOfItems, this.transform.position);
                }
            }
        }

        private void PlayPickUpSound()
        {
            if (pickupAudioSource == null) return;
            pickupAudioSource.Play();
        }

        public bool CanBePickedUp()
        {
            return inventory.HasSpaceFor(inventoryItem);
        }

        public CursorType GetCursorType()
        {
            if (CanBePickedUp())
            {
                return CursorType.Pickup;
            }
            else
            {
                return CursorType.Pickup;
            }
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickupRetriever pickupRetriever = playerController.transform.GetComponent<PickupRetriever>();
                if (pickupRetriever != null)
                {
                    pickupRetriever.StartPickupRetrieval(gameObject);
                }
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag != "Player")  return;

            PickupItem();
        }



    }

}


