using System;
using System.Collections.Generic;
using Base.MVC;
using Managers.SaveLoadManagers;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.Inventory
{
    public class InventoryWindowView : WindowView//<InventoryWindowModel>
    {
        public event Action OnInteractWithItem;
        public event Action OnDrop;
        public event Action OnMove;

        public event Action<ItemCellView> OnClickCell;

        [SerializeField] private ItemCellView _inventoryCellView;
        [Space]
    
        [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
        [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
        [SerializeField] private ActionButtonsView _actionButtonsView;

        private List<ItemCellView> _activeCells = new List<ItemCellView>();
        private List<ItemCellView> _hideCells = new List<ItemCellView>();

        internal void OnEnable()
        {
            _actionButtonsView.OnUseAction += Interact;
            _actionButtonsView.OnEquipAction += Interact;
            _actionButtonsView.OnDropAction += Drop;
            _actionButtonsView.OnMoveAction += Move;
        }

        internal void OnDisable()
        {
            _actionButtonsView.OnUseAction -= Interact;
            _actionButtonsView.OnEquipAction -= Interact;
            _actionButtonsView.OnDropAction -= Drop;
            _actionButtonsView.OnMoveAction -= Move;
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castData = (InventoryWindowModel) uiModel;
        
            _itemInformationPanelView.UpdateView(castData.ItemInformationPanelModel);
            _actionButtonsView.UpdateView(castData.ActionButtonsModel);
            UpdateInventoryCells(castData.InventoryCells);
            base.UpdateView(uiModel);
        }

        private void UpdateInventoryCells(List<InventoryCell> inventoryCells)
        {
            for (var i = 0; i < inventoryCells.Count; i++)
            {
                if (_activeCells.Count == i)
                {
                    if (_hideCells.Count > 0)
                    {
                        var cell = _hideCells[^1];
                        _hideCells.RemoveAt(0);
                        
                        cell.Show();
                        cell.UpdateView(new ItemCellModel(inventoryCells[i].GetItem(),
                            inventoryCells[i].count));
                        
                        _activeCells.Add(cell);
                    }
                    else
                    {
                        CreateCell(inventoryCells[i]);
                    }
                }
                else
                {
                    _activeCells[i].UpdateView(new ItemCellModel(inventoryCells[i].GetItem(),
                        inventoryCells[i].count));
                }
            }

            var inventoryCellsCount = inventoryCells.Count;
            while (_activeCells.Count > inventoryCellsCount)
            {
                _activeCells[inventoryCellsCount].Hide();
                _hideCells.Add(_activeCells[inventoryCellsCount]);
                _activeCells.RemoveAt(inventoryCellsCount);
            }
        }

        private void CreateCell(InventoryCell cell)
        {
            var newCell = Instantiate(_inventoryCellView, _inventoryLayoutGroup.gameObject.transform);
            var cellItem = cell.GetItem();
            newCell.Init(new ItemCellModel(cellItem, cell.count));
            newCell.GetButton().onClick.AddListener(() => ClickOnCell(newCell));
            _activeCells.Add(newCell);
        }
    
        
        
        private void ClickOnCell(ItemCellView cellView)
        { 
            OnClickCell?.Invoke(cellView);
        }
    
        private void Interact()
        {
            OnInteractWithItem?.Invoke();
        }

        private void Drop()
        {
            OnDrop?.Invoke();
        }

        private void Move()
        {
            OnMove?.Invoke();
        }
    }

    public class InventoryWindowModel : UIModel
    {
        public ItemInformationPanelModel ItemInformationPanelModel = new();
        public ActionButtonsModel ActionButtonsModel = new();
        public List<InventoryCell> InventoryCells = new();

        public InventoryWindowModel()
        {
        
        }

        public InventoryWindowModel(List<InventoryCell> inventoryCells,
            ItemInformationPanelModel itemInformationPanelModel = null, ActionButtonsModel actionButtonsModel = null)
        {
            ItemInformationPanelModel = itemInformationPanelModel ?? new ItemInformationPanelModel();
            InventoryCells = inventoryCells;
            ActionButtonsModel = actionButtonsModel ?? new ActionButtonsModel();
        }
    }
}