using System;
using System.Collections.Generic;
using Base;
using UI;
using UI.Inventory;
using UnityEngine;

public class InventoryWindowController : WindowController<InventoryWindowView>
{
    public event Action OnClickCell;
    
    [SerializeField] private ItemCellView _inventoryCellView;
    [SerializeField] private EquipmentPanelController _equipmentPanelController;
    
    private ItemCellView _currentCellView;
    private List<ItemCellView> _cells = new List<ItemCellView>();
    private InventoryData _localInventoryData;
    private void OnEnable()
    {
        _view.OnInteractWithItem += InteractWithItem;
        _view.OnDrop += Drop;
        if (_equipmentPanelController != null)
        {
            _equipmentPanelController.OnContainerClick += ClickOnEquipment;
            _equipmentPanelController.OnRemoveButtonClick += RemoveEquipment;
        }
    }

    private void OnDisable()
    {
        _view.OnInteractWithItem -= InteractWithItem;
        _view.OnDrop -= Drop;
        if (_equipmentPanelController != null)
        {
            _equipmentPanelController.OnContainerClick -= ClickOnEquipment;
            _equipmentPanelController.OnRemoveButtonClick -= RemoveEquipment;
        }
    }

    public override void Show()
    {
        base.Show();
        
        Refresh();

        Inventory.Instance.OnInventoryAdded += AddNewItem;
        Inventory.Instance.OnInventoryDeleted += DeleteItem;
    }

    public override void Hide()
    {
        base.Hide();
        
        Inventory.Instance.OnInventoryAdded -= AddNewItem;
        Inventory.Instance.OnInventoryDeleted -= DeleteItem;
    }

    private void AddNewItem(Item item, int count)
    {
        var newItem = new InventoryCell(item, count);
        CreateCell(newItem);
    }

    private void DeleteItem(Item item, int count) => Refresh();

    private void InteractWithItem()
    {
        RefreshActionButtons();

        var item = _currentCellView.GetItem();
        var count = _currentCellView.GetCount();
        
        if (item is IEquip)
            ((IEquip) item).Equip();
        else if (item is IUse)
            ((IUse) item).Use();

        if (_currentCellView == null) 
            return;
        
        if (count <= 1)
        {
            DestroyCurrentItem();
        }
        else
        {
            count--;
            _currentCellView.SetCount(count);
            Inventory.Instance.DeleteItem(item, 1);
        }
    }

    private void Drop()
    {
        RefreshActionButtons();
        DropCurrentItem();
    }

    private void Refresh()
    {
        _localInventoryData = Inventory.Instance.GetInventory();
        
        RefreshActionButtons();
        _view.SetItemInformation();
        _cells.Clear();
        _currentCellView = null;

        var content = _view.GetInventoryLayout();
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var cell in _localInventoryData.inventoryCells)
        {
            CreateCell(cell);
        }
    }

    private void CreateCell(InventoryCell cell)
    {
        var newCell = Instantiate(_inventoryCellView, _view.GetInventoryLayout().transform);
        var cellItem = cell.GetItem();
        newCell.Init(cellItem, cell.count);
        newCell.GetButton().onClick.AddListener(() => ClickOnCell(newCell));
        _cells.Add(newCell);
    }
    
    private void ClickOnCell(ItemCellView cellView)
    {
        var cellItem = cellView.GetItem();
        _currentCellView = _currentCellView == cellView ? null : cellView;
        if (_currentCellView != null)
            _view.SetItemInformation(cellItem.icon, cellItem.name, cellItem.description);

        RefreshActionButtons(cellItem);
        OnClickCell?.Invoke();
    }

    private void ClickOnEquipment(IEquip equip)
    {
        var item = (Item) equip;
        _view.SetItemInformation(item.icon, item.name, item.description);
        RefreshActionButtons();
    }

    private void RemoveEquipment()
    {
        _view.SetItemInformation();
        RefreshActionButtons();
    }

    private void RefreshActionButtons(Item item = null)
    {
        if (item == null)
        {
            SetActiveActionButton(false);
            return;
        }

        _view.SetActionButtonVisible(item is IUse, item is IEquip,
            true, true);
    }

    private void SetActiveActionButton(bool isActive)
    {
        _view.SetActionButtonVisible(isActive, isActive,
            isActive, isActive);
    }

    private void DropCurrentItem()
    {
        GameBus.Instance.GetPlayer().dropAction?.Invoke(_currentCellView.GetItem(), _currentCellView.GetCount());
        DestroyCurrentItem();
    }

    private void DestroyCurrentItem()
    {
        var item = _currentCellView.GetItem();
        Destroy(_currentCellView.gameObject);
        Inventory.Instance.DeleteItem(item, _currentCellView.GetCount());
        _view.SetItemInformation();
    }
}