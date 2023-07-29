using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : SaveLoadManager<InventoryData, Inventory>
{
    private const string INVENTORY_JSON_PATH = "Inventory.json";

    public event Action<Item, int, InventoryType> OnInventoryAdded;
    public event Action<Item, int, InventoryType> OnInventoryDeleted;

    [Header("Fields for tests")] 
    [SerializeField] private Item _item;
    [SerializeField] private int _count;
    [SerializeField] private InventoryType _inventoryType;

    [ContextMenu("AddItem")]
    private void AddItem()
    {
        AddItem(_item, _count, _inventoryType);
    }

    [ContextMenu("ClearInventory")]
    private void ClearInventory()
    {
        _saveData = new InventoryData();
        Save();
    }

    public void AddItem(Item newItem, int count = 1, InventoryType inventoryType = InventoryType.Inventory)
    {
        var countCopy = count;

        var currentCellsByType = GetInventoryCells(_inventoryType);

        foreach (var inventoryCellWithItem in currentCellsByType.FindAll(cell => cell.GetItem() == newItem))
        {
            if (inventoryCellWithItem.count < newItem.maxStack)
            {
                var remainingSpace = newItem.maxStack - inventoryCellWithItem.count;

                if (remainingSpace >= count)
                {
                    inventoryCellWithItem.count += count;
                    count = 0;
                    break;
                }

                count -= remainingSpace;
                inventoryCellWithItem.count = newItem.maxStack;
            }
        }

        while (count > 0)
        {
            currentCellsByType.Add(new InventoryCell(newItem, math.clamp(count, 1, newItem.maxStack)));
            count -= newItem.maxStack;
        }

        if (inventoryType == InventoryType.Inventory)
            _saveData.inventoryCells = currentCellsByType;
        else
            _saveData.storageCells = currentCellsByType;

        OnInventoryAdded?.Invoke(newItem, countCopy, _inventoryType);
        Save();
    }

    public void DeleteItem(Item item, int count = 1, InventoryType inventoryType = InventoryType.Inventory)
    {
        var deletedCells = new List<InventoryCell>();
        
        var currentCellsByType = GetInventoryCells(_inventoryType);

        foreach (var cell in currentCellsByType.FindAll(cell => cell.GetItem() == item))
        {
            if (cell.count > count)
            {
                cell.count -= count;
                return;
            }

            count -= cell.count;
            deletedCells.Add(cell);

            if (count == 0)
                break;
        }

        foreach (var deletedCell in deletedCells)
            currentCellsByType.Remove(deletedCell);

        if (inventoryType == InventoryType.Inventory)
            _saveData.inventoryCells = currentCellsByType;
        else
            _saveData.storageCells = currentCellsByType;

        OnInventoryDeleted?.Invoke(item, count, _inventoryType);
        Save();
    }
    
    public List<InventoryCell> GetInventoryCells(InventoryType inventoryType = InventoryType.Inventory)
    {
        return inventoryType == InventoryType.Inventory ? _saveData.inventoryCells : _saveData.storageCells;
    }

    public int GetItemCount(Item item, InventoryType inventoryType = InventoryType.Inventory)
    {
        var currentCellsByType = GetInventoryCells(_inventoryType);
        
        var count = 0;
        foreach (var cell in currentCellsByType.Where(cell => cell.GetItem() == item))
        {
            count += cell.count;
        }

        return count;
    }
    protected override void Load()
    {
        base.Load();
        if (_saveData == null)
        {
            _saveData = new InventoryData();
            Save();
        }
    }

    protected override void UpdatePath()
    {
        _secondPath = INVENTORY_JSON_PATH;
        base.UpdatePath();
    }
}

[Serializable]
public class InventoryData
{
    public List<InventoryCell> inventoryCells = new List<InventoryCell>();
    public List<InventoryCell> storageCells = new List<InventoryCell>();
}

[Serializable]
public class InventoryCell
{
    private Item _item;
    public string itemConfigPath;
    public int count;

    public InventoryCell(Item item, int count)
    {
        _item = item;
        itemConfigPath = item.configPath;
        this.count = count;
    }

    public Item GetItem()
    {
        if (_item == null || _item == default)
            _item = Resources.Load<Item>(itemConfigPath);

        return _item;
    }
}

public enum InventoryType
{
    Inventory,
    Storage
}