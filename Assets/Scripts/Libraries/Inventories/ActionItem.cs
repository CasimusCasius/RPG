using UnityEngine;

namespace RPG.Libraries.Inventories
{
    [CreateAssetMenu(menuName = "Game/Inventory/Action")]
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