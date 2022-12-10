using RPG.Libraries.Inventories;
using RPG.Libraries.UI.Dragging;
using UnityEngine;

namespace RPG.Libraries.UI.Inventories
{
    public class InventoryDropTarget : MonoBehaviour, IDragDestination<InventoryItem>
    {
        public void AddItems(InventoryItem item, int number)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ItemDropper>().DropItem(item, number);
        }

        public int MaxAcceptable(InventoryItem items)
        {
            return int.MaxValue;
        }
    }
}