using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class Equipment : MonoBehaviour, ISaveable
    {
        Dictionary<EquipLocation,EquipableItem> equippedItems = new Dictionary<EquipLocation,EquipableItem>();

        public event Action equipmentUpdated;

        public EquipableItem GetItemInSlot(EquipLocation equipLocation)
        {
            if (!equippedItems.ContainsKey(equipLocation)) return null;

            return equippedItems[equipLocation];
        }

        public void AddItem(EquipLocation slot, EquipableItem item)
        {
            if (slot != item.GetAllowedEquipLocation()) return;

            equippedItems.Add(slot, item);
            equipmentUpdated?.Invoke();
        }
        public void RemoveItem(EquipLocation equipLocation)
        {
            equippedItems.Remove(equipLocation);
            equipmentUpdated?.Invoke();
        }

        public object CaptureState()
        {
            var equippedItemsRecords = new Dictionary<EquipLocation, string>();

            foreach (var key in equippedItems.Keys)
            {
                equippedItemsRecords[key] = equippedItems[key].GetItemID();
            }
            return equippedItemsRecords;
        }

        public void RestoreState(object state)
        {
            var equippedItemsRecords = (Dictionary<EquipLocation, string>)state;
            equippedItems.Clear();
            foreach (var key in equippedItemsRecords.Keys)
            {
                var value = InventoryItem.GetFromID(equippedItemsRecords[key]) as EquipableItem;
                if (value != null) {
                    equippedItems[key] = value;
                }
            }
            equipmentUpdated?.Invoke();
        }

       
    }
}
