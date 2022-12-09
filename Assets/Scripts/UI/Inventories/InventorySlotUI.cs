using RPG.UI.Dragging;
using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.Inventories
{

    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon = null;

        int index;
        Inventory inventory;

        public void Setup(Inventory inventory, int index) { 

            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index),GetNumber());

        }
        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }
            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemToSlot(index, item,number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }
        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }
}
