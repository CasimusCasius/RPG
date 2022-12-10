using RPG.Libraries.Inventories;
using RPG.Libraries.UI.Dragging;
using UnityEngine;

namespace RPG.Libraries.UI.Inventories
{
    public class EquipmentSlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] EquipLocation equipLocation;

        Equipment playerEquipment = null;



        //EquipableItem item=null;

        private void Awake()
        {
            var player = GameObject.FindWithTag("Player");
            playerEquipment = player.GetComponent<Equipment>();
        }
        private void OnEnable()
        {
            playerEquipment.equipmentUpdated += RedrawUI;
        }
        private void OnDisable()
        {
            playerEquipment.equipmentUpdated -= RedrawUI;
        }

        private void Start()
        {
            RedrawUI();
        }

        public void AddItems(InventoryItem item, int number)
        {
            playerEquipment.AddItem(equipLocation, (EquipableItem)item);

        }

        public InventoryItem GetItem()
        {

            return playerEquipment.GetItemInSlot(equipLocation);
        }

        public int GetNumber()
        {
            if (GetItem() != null)
                return 1;
            else
                return 0;
        }

        public int MaxAcceptable(InventoryItem item)
        {
            var equipableItem = item as EquipableItem;
            if (equipableItem == null || GetItem() != null)
                return 0;
            if (equipableItem.GetAllowedEquipLocation() != equipLocation)
                return 0;

            return 1;
        }

        public void RemoveItems(int nunber)
        {
            playerEquipment.RemoveItem(equipLocation);
        }

        private void RedrawUI()
        {
            icon.SetItem(playerEquipment.GetItemInSlot(equipLocation));
        }
    }
}