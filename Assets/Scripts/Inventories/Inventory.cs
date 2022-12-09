
using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        public event Action inventoryUpdated;

        [Tooltip("Allowed size")]
        [SerializeField] int invetorySize = 16;

        InventorySlot[] slots;

        public struct InventorySlot
        {
            public InventoryItem item;
            public int number;
        }

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<Inventory>();
        }

        private void Awake()
        {
            slots = new InventorySlot[invetorySize];
        }


        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public int GetSize() => slots.Length;
        public bool AddToFirstEmptySlot(InventoryItem item, int number)
        {
            int i = FindSlot(item);
            if (i < 0) return false;

            slots[i].item = item;
            slots[i].number += number;
            inventoryUpdated?.Invoke();
            return true;

        }
        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (object.ReferenceEquals(slots[i], item)) return true;
            }
            return false;
        }

        public InventoryItem GetItemInSlot(int slot) => slots[slot].item;
        public int GetNumberInSlot(int slot) => slots[slot].number;
        public void RemoveFromSlot(int slot, int number)
        {
            slots[slot].number -= number;
            if (slots[slot].number <= 0)
            {
                slots[slot].number = 0;
                slots[slot].item = null;
            }
            
            inventoryUpdated?.Invoke();
        }
        public bool AddItemToSlot(int slot, InventoryItem item, int number)
        {
            if (slots[slot].item != null) return AddToFirstEmptySlot(item,number);

            var i = FindSlot(item);
            if(i>=0)
            {
                slot = i;
            }

            slots[slot].item = item;
            slots[slot].number += number;
            
            inventoryUpdated?.Invoke();
            return true;
        }

        private int FindSlot(InventoryItem item)
        {
            int i = FindStack(item);
            if (i<0)
            {
                i = FindEmptySlot();
            }

            return i;
        }

        private int FindStack(InventoryItem item)
        {
            if (item.IsStackable())
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (ReferenceEquals(slots[i].item, item)) return i;
                }
            }
            return -1;
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item == null) return i;
            }
            return -1;
        }

        [System.Serializable]
        public struct InventorySlotRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            var recordedSlotsStates = new InventorySlotRecord[invetorySize];
            for (int i = 0; i < invetorySize; i++)
            {
                if (slots[i].item != null)
                {
                    recordedSlotsStates[i].itemID = slots[i].item.GetItemID();
                    recordedSlotsStates[i].number = slots[i].number;
                }
            }
            return recordedSlotsStates;
        }

        public void RestoreState(object state)
        {
            var recordedStates= (InventorySlotRecord[])state;
            for (int i = 0; i < recordedStates.Length; i++)
            {
                slots[i].item = InventoryItem.GetFromID(recordedStates[i].itemID);
                slots[i].number= recordedStates[i].number;
            }
            inventoryUpdated?.Invoke();
        }

    }
}
