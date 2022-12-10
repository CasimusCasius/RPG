using RPG.Libraries.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Libraries.UI.Inventories
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] GameObject container;
        [SerializeField] TextMeshProUGUI itemNumber;

        public void SetItem(InventoryItem item, int number = 1)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {

                iconImage.enabled = false;
                container.SetActive(false);
            }
            else
            {

                iconImage.enabled = true;
                iconImage.sprite = item.GetIcon();

                if (number > 1 && iconImage.enabled)
                {
                    container.SetActive(true);
                    itemNumber.text = number.ToString();
                }
                else
                {
                    container.SetActive(false);
                }

            }
        }
    }
}
