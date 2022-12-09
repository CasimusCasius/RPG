using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI bodyText= null;

        public void Setup(InventoryItem item)
        {
            titleText.text = item.GetDisplayName();
            bodyText.text = item.GetDescription();
        }
    }
}
