using RPG.Inventories;
using RPG.UI.Dragging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public class ActionSlotUi : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] InventoryItemIcon icon = null;
        [SerializeField] int index = 0;

        ActionStore store = null;

        private void Awake()
        {
            store = GameObject.FindGameObjectWithTag("Player").GetComponent<ActionStore>();
        }
        private void OnEnable()
        {
            store.StoreUpdated += RedrawUI;
        }
        private void OnDisable()
        {
            store.StoreUpdated -= RedrawUI;
        }
        private void Start()
        {
            RedrawUI();
        }
        public void AddItems(InventoryItem item, int number)
        {
            store.AddAction((ActionItem)item, index,number);
        }
        public InventoryItem GetItem()
        {
            return store.GetAction(index);
        }
        public int GetNumber()
        {
            return store.GetNumber(index);
        }
        public int MaxAcceptable(InventoryItem item)
        {
            return store.MaxAcceptable(item, index);
        }
        public void RemoveItems(int nunber)
        {
            store.RemoveAction(index, nunber);
        }
        private void RedrawUI()
        {
            icon.SetItem(GetItem(),GetNumber());
        }
    }
}