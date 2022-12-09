using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public class ActionStore : MonoBehaviour, ISaveable
    {
        private class DockedItemSlot
        {
            public ActionItem item;
            public int number;
        }

        Dictionary<int,DockedItemSlot> dockedItems = new Dictionary<int,DockedItemSlot>();
        public event Action StoreUpdated;

        public InventoryItem GetAction(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].item;
            }
            return null;
        }
        public int GetNumber(int index)
        {
            if (dockedItems.ContainsKey(index))
            {
                return dockedItems[index].number;
            }
            return 0;
        }

        public void AddAction(ActionItem item, int index, int number)
        {
            if (dockedItems.ContainsKey(index) && ReferenceEquals(dockedItems[index].item,item))
            {
                dockedItems[index].number += number;
            }
            else
            {
                var itemToDock = new DockedItemSlot();
                itemToDock.item = item as ActionItem;
                itemToDock.number = number;
                dockedItems[index] = itemToDock;
            }
            StoreUpdated?.Invoke();
        }

        public bool Use(int index, GameObject user)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].item.Use(user);
                if (dockedItems[index].item.IsConsumable())
                {
                    RemoveAction(index, 1);
                }
                return true;
            }
            return false;
        }

        public void RemoveAction(int index, int number)
        {
            if (dockedItems.ContainsKey(index))
            {
                dockedItems[index].number -= number;

                if (dockedItems[index].number <= 0)
                {
                    //dockedItems[index].number = 0;
                    //dockedItems[index].item = null;
                    dockedItems.Remove(index);
                }
               
            }
            StoreUpdated?.Invoke();
        }

        public int MaxAcceptable(InventoryItem item, int index)
        {
            var actionItem = item as ActionItem;
            if(!actionItem) return 0;
            if (dockedItems.ContainsKey(index) && !object.ReferenceEquals(item, dockedItems[index].item)) return 0;
            if (actionItem.IsConsumable()) return int.MaxValue;
            if (dockedItems.ContainsKey(index)) return 0;
            return 1;
            
        }

        [System.Serializable]
        private struct DockedItemRecord
        {
            public string itemID;
            public int number;
        }

        public object CaptureState()
        {
            Dictionary<int,DockedItemRecord> states = new Dictionary<int,DockedItemRecord>();
            foreach (var item in dockedItems)
            {
                var dockedRecord = new DockedItemRecord();
                dockedRecord.itemID = item.Value.item.GetItemID();
                dockedRecord.number = item.Value.number;
                states[item.Key] = dockedRecord;
            }
            return states;
        }

        public void RestoreState(object state)
        {
            var dockedRecords = (Dictionary<int, DockedItemRecord>) state;
            dockedItems.Clear();
            foreach (var item in dockedRecords)
            {
                var itemToRestore = new DockedItemSlot();
                itemToRestore.number = item.Value.number;
                itemToRestore.item = InventoryItem.GetFromID(item.Value.itemID) as ActionItem;
                dockedItems[item.Key] = itemToRestore;
            }

            StoreUpdated?.Invoke();
        }
    }
}
