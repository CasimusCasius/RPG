using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Inventories
{
    public class Pickup : MonoBehaviour
    {
        InventoryItem item;
        int number;

        Inventory inventory;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            inventory = player.GetComponent<Inventory>();
        }
        public void Setup(InventoryItem item, int number)
        {
            this.item = item;
            this.number = number;
        }
        public InventoryItem GetItem() => item;
        public int GetNumber() => number;
        public void PickupItem()
        {
            bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
            if (foundSlot)
            {
                Destroy(gameObject);
            }
        }

        public bool CanBePickedUp()=> inventory.HasSpaceFor(item);
       

    }
}
