using RPG.Inventories;
using RPG.UI.Tooltip;
using UnityEngine;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>().GetItem();
            if (!item) return false;
            return true;
        }
        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip) return;

            InventoryItem item = GetComponent<IItemHolder>().GetItem();

            itemTooltip.Setup(item);
        }
    }
}
