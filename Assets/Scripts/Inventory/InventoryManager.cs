using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Base
{
    public class InventoryManager : SaveLoadManager<Inventory, InventoryManager>
    {
        private const string INVENTORY_JSON_PATH = "Inventory.json";

        [Header("Fields for tests")]
        [SerializeField] private Item _item;
        [SerializeField] private int _count;

        [ContextMenu("AddItem")]
        private void AddItem()
        {
            AddItem(_item, _count);   
        }

        [ContextMenu("ClearInventory")]
        private void ClearInventory()
        {
            _saveData = new Inventory();
            Save();
        }
        
        public void AddItem(Item newItem, int count = 1)
        {
            foreach (var inventoryCellWithItem in _saveData.inventoryCells.FindAll(cell => cell.GetItem() == newItem))
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
                _saveData.inventoryCells.Add(new InventoryCell(newItem,math.clamp(count, 1,newItem.maxStack)));
                count -= newItem.maxStack;
            }
            
            Save();
        }

        public void DeleteItem(Item item, int count = 1)
        {
            var deletedCells = new List<InventoryCell>();
            foreach (var cell in _saveData.inventoryCells.FindAll(cell => cell.GetItem() == item))
            {
                if (cell.count > count)
                {
                    cell.count -= count;
                    return;
                }

                count -= cell.count;
                deletedCells.Add(cell);

                if (count == 0)
                    return;
            }

            foreach (var deletedCell in deletedCells) 
                _saveData.inventoryCells.Remove(deletedCell);
            
            Save();
        }
        
        public void DeleteItem(int itemIndex)
        {
            _saveData.inventoryCells.RemoveAt(itemIndex);
            
            Save();
        }

        public Inventory GetInventory() => _saveData;

        protected override void Load()
        {
            base.Load();
            if (_saveData == null)
            {
                _saveData = new Inventory();
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
    public class Inventory
    {
        public List<InventoryCell> inventoryCells = new List<InventoryCell>();
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
}