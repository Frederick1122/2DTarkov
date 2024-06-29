using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using ConfigScripts;
using Managers.Libraries;
using Unity.Mathematics;
using UnityEngine;

namespace Managers.SaveLoadManagers
{
    public class InventorySaveLoadManager : SaveLoadManager<InventoryData, InventorySaveLoadManager>
    {
        private const string INVENTORY_JSON_PATH = "Inventory.json";

        public event Action<ItemConfig, int, InventoryType> OnInventoryAdded;
        public event Action<ItemConfig, int, InventoryType> OnInventoryDeleted;

        [Header("Fields for tests")] 
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private int _count;
        [SerializeField] private InventoryType _inventoryType;

        [ContextMenu("AddItem")]
        private void AddItem()
        {
            AddItem(_itemConfig, _count, _inventoryType);
        }

        [ContextMenu("ClearInventory")]
        private void ClearInventory()
        {
            _saveData = new InventoryData();
            Save();
        }

        public void AddItem(ItemConfig newItemConfig, int count = 1, InventoryType inventoryType = InventoryType.Inventory)
        {
            var countCopy = count;

            var currentCellsByType = GetInventoryCells(inventoryType);

            foreach (var inventoryCellWithItem in currentCellsByType.FindAll(cell => cell.GetItem() == newItemConfig))
            {
                if (inventoryCellWithItem.count < newItemConfig.maxStack)
                {
                    var remainingSpace = newItemConfig.maxStack - inventoryCellWithItem.count;

                    if (remainingSpace >= count)
                    {
                        inventoryCellWithItem.count += count;
                        count = 0;
                        break;
                    }

                    count -= remainingSpace;
                    inventoryCellWithItem.count = newItemConfig.maxStack;
                }
            }

            while (count > 0)
            {
                currentCellsByType.Add(new InventoryCell(newItemConfig, math.clamp(count, 1, newItemConfig.maxStack)));
                count -= newItemConfig.maxStack;
            }

            if (inventoryType == InventoryType.Inventory)
                _saveData.inventoryCells = currentCellsByType;
            else
                _saveData.storageCells = currentCellsByType;

            OnInventoryAdded?.Invoke(newItemConfig, countCopy, inventoryType);
            Save();
        }

        public void DeleteItem(ItemConfig itemConfig, int count = 1, InventoryType inventoryType = InventoryType.Inventory)
        {
            var deletedCells = new List<InventoryCell>();
        
            var currentCellsByType = GetInventoryCells(inventoryType);

            foreach (var cell in currentCellsByType.FindAll(cell => cell.GetItem() == itemConfig))
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

            OnInventoryDeleted?.Invoke(itemConfig, count, _inventoryType);
            Save();
        }
    
        public List<InventoryCell> GetInventoryCells(InventoryType inventoryType = InventoryType.Inventory)
        {
            return inventoryType == InventoryType.Inventory ? _saveData.inventoryCells : _saveData.storageCells;
        }

        public int GetItemCount(ItemConfig itemConfig, InventoryType inventoryType = InventoryType.Inventory)
        {
            var currentCellsByType = GetInventoryCells(inventoryType);
        
            var count = 0;
            foreach (var cell in currentCellsByType.Where(cell => cell.GetItem() == itemConfig))
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
        private ItemConfig _config;
        public string configKey;
        public int count;

        public InventoryCell(ItemConfig config, int count)
        {
            _config = config;
            configKey = config.configKey;
            this.count = count;
        }

        public ItemConfig GetItem()
        {
            if (_config == null || _config == default)
                _config = ItemLibrary.Instance.GetConfig(configKey);

            return _config;
        }
    }

    public enum InventoryType
    {
        Inventory,
        Storage,
        LootBox
    }
}