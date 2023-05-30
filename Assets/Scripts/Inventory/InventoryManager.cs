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
                if (inventoryCellWithItem.Count < newItem.maxStack)
                {
                    var remainingSpace = newItem.maxStack - inventoryCellWithItem.Count;

                    if (remainingSpace >= count)
                    {
                        inventoryCellWithItem.Count += count;
                        count = 0;
                        break;
                    }

                    count -= remainingSpace;
                    inventoryCellWithItem.Count = newItem.maxStack;
                }
            }
            Save();
        }

        public void DeleteItem(Item item, int count)
        {
            var deletedCells = new List<InventoryCell>();
            foreach (var cell in _saveData.inventoryCells.FindAll(cell => cell.GetItem() == item))
            {
                if (cell.Count > count)
                {
                    cell.Count -= count;
                    return;
                }

                count -= cell.Count;
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
        
        private void Start() => Load();

        private void OnApplicationQuit() => Save();

        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
                Save();
        }
        
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
        public string ItemConfigPath;
        public int Count;

        public InventoryCell(Item item, int count)
        {
            _item = item;
            ItemConfigPath = item.configPath;
            Count = count;
        }

        public Item GetItem()
        {
            if (_item == null || _item == default) 
                _item = Resources.Load<Item>(ItemConfigPath);
        
            return _item;
        }
    }
}