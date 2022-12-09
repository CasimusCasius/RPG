using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "Inventory/Action")]
    public class ActionItem : InventoryItem
    {
        [SerializeField] bool consumable = false;

        public virtual void Use(GameObject user)
        {
            Debug.Log("Using action: " + this);
        }

        public bool IsConsumable() => consumable;
       
    }
}