using RPG.Libraries.Inventories;
using UnityEngine;

namespace RPG.Libraries.UI.Inventories
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] InventorySlotUI inventorySlotPrefab = null;

        Inventory playerInventory;

        private void Awake()
        {
            playerInventory = Inventory.GetPlayerInventory();

        }
        private void OnEnable()
        {
            playerInventory.inventoryUpdated += Redraw;
        }
        private void OnDisable()
        {
            playerInventory.inventoryUpdated -= Redraw;
        }
        private void Start()
        {
            Redraw();
        }

        public void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerInventory.GetSize(); i++)
            {
                var itemUI = Instantiate(inventorySlotPrefab, transform);
                itemUI.Setup(playerInventory, i);
            }
        }


    }
}
