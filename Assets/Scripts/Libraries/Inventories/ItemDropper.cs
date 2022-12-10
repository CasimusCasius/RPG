using RPG.Libraries.Saving;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Libraries.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> droppedItems = new List<Pickup>();
        private List<DropRecord> otherSceneDroppedItems = new List<DropRecord>();
        public void DropItem(InventoryItem item, int number)
        {
            SpawnPickup(item, GetDropLocation(), number);
        }
        protected virtual Vector3 GetDropLocation()
        {
            return gameObject.transform.position;
        }
        private void SpawnPickup(InventoryItem item, Vector3 spawnLocation, int number)
        {
            if (item == null) return;
            var pickup = item.SpawnPickup(spawnLocation, number);
            droppedItems.Add(pickup);
        }
        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
            public int number;
            public int sceneBuildIndex;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemsList = new List<DropRecord>();
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            foreach (Pickup pickup in droppedItems)
            {
                var droppedItem = new DropRecord();
                droppedItem.itemID = pickup.GetItem().GetItemID();
                droppedItem.position = new SerializableVector3(pickup.transform.position);
                droppedItem.number = pickup.GetNumber();
                droppedItem.sceneBuildIndex = buildIndex;
                droppedItemsList.Add(droppedItem);
            }
            droppedItemsList.AddRange(otherSceneDroppedItems);
            return droppedItemsList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemsList = (List<DropRecord>)state;
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            otherSceneDroppedItems.Clear();
            foreach (var item in droppedItemsList)
            {
                if (item.sceneBuildIndex != buildIndex)
                {
                    otherSceneDroppedItems.Add(item);
                    continue;
                }
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector3();
                int number = item.number;
                SpawnPickup(pickupItem, position, number);
            }
        }

        /// <summary>
        /// Remove any drops in the world that have subsequently been picked up.
        /// </summary>
        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }
            droppedItems = newList;
        }
    }
}
