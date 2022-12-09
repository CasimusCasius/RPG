using RPG.Inventories;
using RPG.UI.Dragging;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item,number);
        }

        public int MaxAcceptable(InventoryItem items)
        {
            return int.MaxValue;
        }
    }
}