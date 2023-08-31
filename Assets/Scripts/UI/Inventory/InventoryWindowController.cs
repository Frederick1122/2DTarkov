using System;
using System.Collections.Generic;
using Base;
using UI;
using UI.Inventory;
using UnityEngine;

public class InventoryWindowController : WindowController<InventoryWindowView, InventoryWindowModel>
{
    public event Action OnClickCell;

    [SerializeField] private ItemCellView _inventoryCellView;
    [SerializeField] private EquipmentPanelController _equipmentPanelController;
    [SerializeField] private LootBoxWindowController _lootBoxWindowController;

    private ItemCellView _currentCellView;
    private List<ItemCellView> _cells = new List<ItemCellView>();
    private List<InventoryCell> _inventoryCells;
    private InventoryType _inventoryType;
    private bool _isStorageWindow;

    private void OnEnable()
    {
        _view.OnInteractWithItem += InteractWithItem;
        _view.OnDrop += Drop;
        _view.OnMove += Move;

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
        _view.OnMove -= Move;

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

        Inventory.Instance.OnInventoryAdded += AddNewItem;
        Inventory.Instance.OnInventoryDeleted += DeleteItem;
    }

    public override void Hide()
    {
        base.Hide();

        Inventory.Instance.OnInventoryAdded -= AddNewItem;
        Inventory.Instance.OnInventoryDeleted -= DeleteItem;
    }

    private void AddNewItem(Item item, int count, InventoryType inventoryType)
    {
        if (inventoryType != _inventoryType)
            return;

        var newItem = new InventoryCell(item, count);
        CreateCell(newItem);
    }

    private void DeleteItem(Item item, int count, InventoryType inventoryType)
    {
        if (inventoryType == _inventoryType)
            Refresh();
    }

    private void InteractWithItem()
    {
        if (_currentCellView == null)
            return;

        RefreshActionButtons();

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
            Inventory.Instance.DeleteItem(item, 1, _inventoryType);
        }
    }

    private void Drop()
    {
        if (_currentCellView == null)
            return;

        RefreshActionButtons();

        GameBus.Instance.GetPlayer().dropAction?.Invoke(_currentCellView.GetItem(), _currentCellView.GetCount());
        DestroyCurrentItem();
    }

    private void Move()
    {
        if (_currentCellView == null)
            return;

        RefreshActionButtons();

        if (_lootBoxWindowController != null)
        {
            
        }
        
        var inventoryTypeToTransfer =
            _inventoryType == InventoryType.Inventory ? InventoryType.Storage : InventoryType.Inventory;
        Inventory.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount(), inventoryTypeToTransfer);
        DestroyCurrentItem();
    }

    private void Refresh()
    {
        _inventoryCells = Inventory.Instance.GetInventoryCells(_inventoryType);

        RefreshActionButtons();
        _view.UpdateView(new InventoryWindowModel());
        _cells.Clear();
        _currentCellView = null;

        var content = _view.GetInventoryLayout();
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var cell in _inventoryCells)
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
            _view.UpdateView(new InventoryWindowModel(cellItem.icon, cellItem.name, cellItem.description));

        RefreshActionButtons(cellItem);
        OnClickCell?.Invoke();
    }

    private void ClickOnEquipment(IEquip equip)
    {
        var item = (Item) equip;
        _view.UpdateView(new InventoryWindowModel(item.icon, item.name, item.description));
        RefreshActionButtons();
    }

    private void RemoveEquipment()
    {
        _view.UpdateView(new InventoryWindowModel());
        RefreshActionButtons();
    }

    private void RefreshActionButtons(Item item = null)
    {
        if (item == null)
        {
            _view.UpdateView(new InventoryWindowModel());
            return;
        }

        _view.UpdateView(new InventoryWindowModel(new ItemInformationPanelModel(item.icon, item.name, item.description),
            item is IUse, item is IEquip,
            true, !_isStorageWindow, _isStorageWindow || _lootBoxWindowController != null));
    }

    private void DestroyCurrentItem()
    {
        if (_currentCellView == null)
            return;

        var item = _currentCellView.GetItem();
        Destroy(_currentCellView.gameObject);
        Inventory.Instance.DeleteItem(item, _currentCellView.GetCount(), _inventoryType);
        _view.UpdateView(new InventoryWindowModel());
    }
}