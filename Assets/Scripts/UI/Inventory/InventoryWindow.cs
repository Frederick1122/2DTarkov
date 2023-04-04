using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryWindow : MonoBehaviour
    {
        [SerializeField] private InventoryWindowCell _inventoryCell;
        [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
        [SerializeField] private ItemInformationPanel _itemInformationPanel;
        
        [Space] [Header("Action Buttons")]
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _divideButton;
        [SerializeField] private Button _dropButton;

        private InventoryWindowCell _currentCell;
        private Dictionary<InventoryWindowCell, int> _cells = new Dictionary<InventoryWindowCell, int>();

        private void Start()
        {
            _useButton.onClick.AddListener(() => {
            {
                RefreshActionButtons();
                
                var item = _currentCell.GetItem();
                if (item is IUse)
                    ((IUse)item).Use();
            }});
            
            _equipButton.onClick.AddListener(() => {
            {
                RefreshActionButtons();
                
                var item = _currentCell.GetItem();
                if (item is IEquip)
                    ((IEquip)item).Equip();
            }});
            
            _dropButton.onClick.AddListener(() =>
            {
                //TODO: Destroy button, clean inventory, spawn drop in spawnpoint
                
                RefreshActionButtons();
                DropCurrentItem();
            });
        }

        public void Refresh()
        {
            RefreshActionButtons();
            _itemInformationPanel.SetNewInformation();
            _cells.Clear();
            _currentCell = null;
            
            var content = _inventoryLayoutGroup.gameObject;
            foreach (Transform child in content.transform) {
                Destroy(child.gameObject);
            }

            var inventory = InventoryManager.Instance.GetInventory();
            var counter = 0;
            foreach (var cell in inventory.inventoryCells)
            {
                var newCell = Instantiate(_inventoryCell, _inventoryLayoutGroup.transform);
                var cellItem = cell.GetItem(); 
                newCell.Init(cellItem, cell.Count);
                newCell.GetButton().onClick.AddListener(() => ClickOnCell(newCell));
                _cells.Add(newCell, counter);
            }
        }

        private void ClickOnCell(InventoryWindowCell cell)
        {
            var cellItem = cell.GetItem(); 
            _currentCell = _currentCell == cell ? null : cell;
            if (_currentCell != null) 
                _itemInformationPanel.SetNewInformation(cellItem.icon, cellItem.name, cellItem.description);

            RefreshActionButtons(cellItem);
        }
        
        private void RefreshActionButtons(Item item = null)
        {
            if (item == null)
            {
                SetActiveActionButton(false);
                return;
            }

            _useButton.gameObject.SetActive(item is IUse); 
            _equipButton.gameObject.SetActive(item is IEquip);
            
            _divideButton.gameObject.SetActive(true);
            _dropButton.gameObject.SetActive(true);
        }

        private void SetActiveActionButton(bool isActive)
        {
            _useButton.gameObject.SetActive(isActive);
            _equipButton.gameObject.SetActive(isActive);
            _divideButton.gameObject.SetActive(isActive);
            _dropButton.gameObject.SetActive(isActive);
        }

        private void DropCurrentItem()
        {
            GameManager.Instance.GetPlayer().dropAction?.Invoke(_currentCell.GetItem(), _currentCell.GetCount());
            InventoryManager.Instance.DeleteItem(_cells[_currentCell], _currentCell.GetCount());
            Destroy(_currentCell.gameObject);
        }
    }
}
