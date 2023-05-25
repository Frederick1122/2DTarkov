using System;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryWindowController : WindowController
    {
        [SerializeField] private InventoryWindowCellView _inventoryCellView;
        [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
        [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
        [SerializeField] private InventoryActionButtonsView _inventoryActionButtonsView;

        private InventoryWindowCellView _currentCellView;
        private Dictionary<InventoryWindowCellView, int> _cells = new Dictionary<InventoryWindowCellView, int>();

        private void Start()
        {
            _inventoryActionButtonsView.OnUseAction += Use;
            _inventoryActionButtonsView.OnEquipAction += Equip;
            _inventoryActionButtonsView.OnDropAction += Drop;
        }

        private void OnDestroy()
        {
            _inventoryActionButtonsView.OnUseAction -= Use;
            _inventoryActionButtonsView.OnEquipAction -= Equip;
            _inventoryActionButtonsView.OnDropAction -= Drop;
        }

        private void Use()
        {
            RefreshActionButtons();
                
            var item = _currentCellView.GetItem();
            if (item is IUse)
                ((IUse)item).Use();
        }

        private void Equip()
        {
            RefreshActionButtons();
                
            var item = _currentCellView.GetItem();
            if (item is IEquip)
                ((IEquip)item).Equip(); 
        }
        
        private void Drop()
        { 
            RefreshActionButtons();
            DropCurrentItem();
        }

        public void Refresh()
        {
            RefreshActionButtons();
            _itemInformationPanelView.SetNewInformation();
            _cells.Clear();
            _currentCellView = null;
            
            var content = _inventoryLayoutGroup.gameObject;
            foreach (Transform child in content.transform) {
                Destroy(child.gameObject);
            }

            var inventory = InventoryManager.Instance.GetInventory();
            var counter = 0;
            foreach (var cell in inventory.inventoryCells)
            {
                var newCell = Instantiate(_inventoryCellView, _inventoryLayoutGroup.transform);
                var cellItem = cell.GetItem(); 
                newCell.Init(cellItem, cell.Count);
                newCell.GetButton().onClick.AddListener(() => ClickOnCell(newCell));
                _cells.Add(newCell, counter);
            }
        }

        private void ClickOnCell(InventoryWindowCellView cellView)
        {
            var cellItem = cellView.GetItem(); 
            _currentCellView = _currentCellView == cellView ? null : cellView;
            if (_currentCellView != null) 
                _itemInformationPanelView.SetNewInformation(cellItem.icon, cellItem.name, cellItem.description);

            RefreshActionButtons(cellItem);
        }

        private void RefreshActionButtons(Item item = null)
        {
            if (item == null)
            {
                SetActiveActionButton(false);
                return;
            }

            _inventoryActionButtonsView.SetActiveButtons(item is IUse, item is IEquip,
                true, true);
        }

        private void SetActiveActionButton(bool isActive)
        {
            _inventoryActionButtonsView.SetActiveButtons(isActive, isActive,
                isActive, isActive);
        }

        private void DropCurrentItem()
        {
            GameManager.Instance.GetPlayer().dropAction?.Invoke(_currentCellView.GetItem(), _currentCellView.GetCount());
            InventoryManager.Instance.DeleteItem(_cells[_currentCellView], _currentCellView.GetCount());
            Destroy(_currentCellView.gameObject);
        }
    }
}
