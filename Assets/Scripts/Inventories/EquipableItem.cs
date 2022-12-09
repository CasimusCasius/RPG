using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "Inventory/Equipment")]
    public class EquipableItem : InventoryItem
    {
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
    }
}
