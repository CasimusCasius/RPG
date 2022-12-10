using RPG.Libraries.UI.Inventories;
using UnityEngine;


namespace RPG.Libraries.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode showHideKey = KeyCode.I;
        [SerializeField] GameObject objectToHide = null;
        [SerializeField] InventoryUI inventorySlots = null;

        private void Start()
        {
            objectToHide.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKeyDown(showHideKey))
            {
                ShowHide();
            }

        }

        private void ShowHide()
        {

            objectToHide.SetActive(!objectToHide.activeSelf);
            inventorySlots.Redraw();
        }
    }
}
