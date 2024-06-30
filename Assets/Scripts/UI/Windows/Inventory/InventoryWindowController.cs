using System;
using System.Collections.Generic;
using Base;
using Base.MVC;
using ConfigScripts;
using Managers.SaveLoadManagers;
using UI.Inventory;
using UnityEditor;
using UnityEngine;

namespace UI.Windows.Inventory
{
    public class InventoryWindowController : WindowController//<InventoryWindowView, InventoryWindowModel>
    {
        public event Action OnClickCell;

        [SerializeField] private EquipmentPanelController _equipmentPanelController;
        [SerializeField] private LootBoxWindowController _lootBoxWindowController;

        private InventoryWindowModel _model = new();
        private ItemConfig _currentEquipmentItem;
        private ItemCellView _currentCellView;
        private List<InventoryCell> _inventoryCells;
        private InventoryType _inventoryType;
        private bool _isStorageWindow;

        private void OnEnable()
        {
            GetView<InventoryWindowView>().OnInteractWithItem += InteractWithItem;
            GetView<InventoryWindowView>().OnDrop += Drop;
            GetView<InventoryWindowView>().OnMove += Move;
            GetView<InventoryWindowView>().OnClickCell += ClickOnCell;

            if (_equipmentPanelController != null)
            {
                _equipmentPanelController.OnContainerClick += ClickOnEquipment;
                _equipmentPanelController.OnRemoveButtonClick += RemoveEquipment;
            }
        }

        private void OnDisable()
        {
            GetView<InventoryWindowView>().OnInteractWithItem -= InteractWithItem;
            GetView<InventoryWindowView>().OnDrop -= Drop;
            GetView<InventoryWindowView>().OnMove -= Move;
            GetView<InventoryWindowView>().OnClickCell -= ClickOnCell;

            if (_equipmentPanelController != null)
            {
                _equipmentPanelController.OnContainerClick -= ClickOnEquipment;
                _equipmentPanelController.OnRemoveButtonClick -= RemoveEquipment;
            }
        }

        public void Init(InventoryType inventoryType, bool isStorageWindow)
        {
            _inventoryType = inventoryType;
            _isStorageWindow = isStorageWindow;
            base.Init();
        }

        public void ClearCurrentCell()
        {
            _currentCellView = null;
        }

        public override void Show()
        {
            base.Show();

            Refresh();

            InventorySaveLoadManager.Instance.OnInventoryAdded += AddNewItem;
            InventorySaveLoadManager.Instance.OnInventoryDeleted += DeleteItem;
        }

        public override void Hide()
        {
            base.Hide();

            InventorySaveLoadManager.Instance.OnInventoryAdded -= AddNewItem;
            InventorySaveLoadManager.Instance.OnInventoryDeleted -= DeleteItem;
        }

        protected override UIModel GetViewData()
        {
            var currentItem = _currentCellView != null ? _currentCellView.GetItem() : _currentEquipmentItem;
            SetModels(currentItem);
        
            _model.InventoryCells = _inventoryCells;
        
            return _model;
        }

        private void SetModels(ItemConfig item = null)
        {
            if (item != null)
            {
                _model.ItemInformationPanelModel.icon = item.icon;
                _model.ItemInformationPanelModel.name = item.name;
                _model.ItemInformationPanelModel.description = item.description;

                _model.ActionButtonsModel = new ActionButtonsModel(item is IUse, item is IEquip,
                    true, !_isStorageWindow, _isStorageWindow || _lootBoxWindowController != null);
            }
            else
            {
                _model.ItemInformationPanelModel = new ItemInformationPanelModel();
                _model.ActionButtonsModel = new ActionButtonsModel();
            }
        }
    
        private void AddNewItem(ItemConfig item, int count, InventoryType inventoryType)
        {
            if (inventoryType != _inventoryType)
                return;

            _inventoryCells.Add( new InventoryCell(item, count));
            UpdateView();
        }

        private void DeleteItem(ItemConfig item, int count, InventoryType inventoryType)
        {
            if (inventoryType == _inventoryType)
                Refresh();
        }

        private void InteractWithItem()
        {
            if (_currentCellView == null)
                return;

            var item = _currentCellView.GetItem();
            var count = _currentCellView.GetCount();

            if (item is IEquip)
                ((IEquip) item).Equip();
            else if (item is IUse)
                ((IUse) item).Use();

            if (count <= 1)
            {
                DestroyCurrentItem();
            }
            else
            {
                count--;
                _currentCellView.SetCount(count);
                InventorySaveLoadManager.Instance.DeleteItem(item, 1, _inventoryType);
            }
        }

        private void Drop()
        {
            if (_currentCellView == null)
                return;
            
            GameBus.Instance.PlayerHumanoid.dropAction?.Invoke(_currentCellView.GetItem(), _currentCellView.GetCount());
            DestroyCurrentItem();
        }

        private void Move()
        {
            if (_currentCellView == null)
                return;
            
            if (_lootBoxWindowController != null)
            {
            
            }
        
            var inventoryTypeToTransfer =
                _inventoryType == InventoryType.Inventory ? InventoryType.Storage : InventoryType.Inventory;
            InventorySaveLoadManager.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount(), inventoryTypeToTransfer);
            DestroyCurrentItem();
        }

        private void Refresh()
        {
            _inventoryCells = InventorySaveLoadManager.Instance.GetInventoryCells(_inventoryType);
            UpdateView();
        }

        private void ClickOnCell(ItemCellView cellView)
        {
            _currentCellView = _currentCellView == cellView ? null : cellView;
        
            UpdateView();
            OnClickCell?.Invoke();
        }

        private void ClickOnEquipment(IEquip equip)
        {
            _currentEquipmentItem  = (ItemConfig) equip;
            UpdateView();
        }

        private void RemoveEquipment()
        {
            // _view.UpdateView(new InventoryWindowModel());
            // RefreshActionButtons();
        }

        private void DestroyCurrentItem()
        {
            if (_currentCellView == null)
                return;

            InventorySaveLoadManager.Instance.DeleteItem(_currentCellView.GetItem(), _currentCellView.GetCount(), _inventoryType);
        }
    }
}