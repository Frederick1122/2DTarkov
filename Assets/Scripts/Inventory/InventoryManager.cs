using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace Base
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        private const string INVENTORY_JSON_PATH = "/Inventory.json";

        private Inventory _inventory;
        private string _path = "";

        [Header("Fields for tests")]
        [SerializeField] private Item _item;
        [SerializeField] private int _count;

        [ContextMenu("AddItem")]
        private void AddItem()
        {
            AddItem(_item, _count);   
        }
        
        public void AddItem(Item newItem, int count = 1)
        {
            foreach (var inventoryCellWithItem in _inventory.inventoryCells.FindAll(cell => cell.GetItem() == newItem))
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

            while (count > 0)
            {
                _inventory.inventoryCells.Add(new InventoryCell(newItem,math.clamp(count, 1,newItem.maxStack)));
                count -= newItem.maxStack;
            }
            
            Save();
        }

        public void DeleteItem(Item item, int count)
        {
            var deletedCells = new List<InventoryCell>();
            foreach (var cell in _inventory.inventoryCells.FindAll(cell => cell.GetItem() == item))
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
                _inventory.inventoryCells.Remove(deletedCell);
            
            Save();
        }
        
        public void DeleteItem(int itemIndex)
        {
            _inventory.inventoryCells.RemoveAt(itemIndex);
            
            Save();
        }

        public Inventory GetInventory() => _inventory;
        
        private void Start()
        {
            Load();
        }

        public void Load()
        {
            UpdatePath();
            _inventory = JsonUtility.FromJson<Inventory>(File.ReadAllText(_path));
        }

        public void Save()
        {
            UpdatePath();
            File.WriteAllText(_path, JsonUtility.ToJson(_inventory));
        }

        private void UpdatePath()
        {
            if (_path == "")
                _path = Application.streamingAssetsPath + INVENTORY_JSON_PATH;
        }

        private void OnApplicationQuit() => Save();

        private void OnApplicationPause(bool pauseStatus)
        {
            if(pauseStatus)
                Save();
        }
    }

    [Serializable]
    public class Inventory
    {
        public List<InventoryCell> inventoryCells;
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