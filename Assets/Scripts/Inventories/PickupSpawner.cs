using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{

    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] InventoryItem item = null;
        [SerializeField] int number = 1;
        private void Awake()
        {
            SpawnPickup();
        }
        public Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }
        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, number);
            spawnedPickup.transform.SetParent(transform);
        }

        private bool isCollected()
        {
            return GetPickup() == null;
        }
        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        public object CaptureState()
        {
            return isCollected();
        }

        
        public void RestoreState(object state)
        {
            bool shouldBeCollected = (bool)state;

            if (shouldBeCollected && !isCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && isCollected())
            {
                SpawnPickup();
            }
        }

        
    }
}
