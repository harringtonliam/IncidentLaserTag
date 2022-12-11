using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Combat;

namespace RPG.InventoryControl
{

    public class Pickup : MonoBehaviour
    {
        //Config
        [SerializeField] InventoryItem inventoryItem = null;
        [SerializeField] int numberOfItems = 1;

        //Cached references
        Inventory inventory;

        public InventoryItem InventoryItem { get { return inventoryItem; } }
        public int NumberOfItems { get { return numberOfItems; } }

        GameObject player;


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
            bool slotFoundOk = true;
            Ammunition ammunitionItem = (Ammunition)inventoryItem;


            if (ammunitionItem != null)
            {
                Debug.Log("adding ammo to slot");
                slotFoundOk = player.GetComponent<AmmunitionStore>().AddToFirstEmptySlot(ammunitionItem, numberOfItems);
            }
            else
            {
                slotFoundOk = inventory.AddToFirstEmptySlot(inventoryItem, numberOfItems);
            }

            if (slotFoundOk)
            {
                Destroy(gameObject);
                ScenePickups scenePickups = FindObjectOfType<ScenePickups>();
                if (scenePickups != null)
                {
                    scenePickups.RemoveItem(this.inventoryItem, this.numberOfItems, this.transform.position);
                }
            }
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

            Debug.Log("Ammo pickup triggered");
            PickupItem();
        }



    }

}


