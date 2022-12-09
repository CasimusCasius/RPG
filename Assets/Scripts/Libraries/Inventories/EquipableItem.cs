using UnityEngine;

namespace RPG.Libraries.Inventories
{
    [CreateAssetMenu(menuName = "Game/Inventory/Equipment")]
    public class EquipableItem : InventoryItem
    {
        [SerializeField] EquipLocation allowedEquipLocation = EquipLocation.Weapon;

        public EquipLocation GetAllowedEquipLocation()
        {
            return allowedEquipLocation;
        }
    }
}
