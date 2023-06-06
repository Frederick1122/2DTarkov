﻿using System;
using Base;
using UnityEngine;

public class Equipment: SaveLoadManager<EquipmentData, Equipment>
    {   
        private const string EQUIPMENT_JSON_PATH = "Equipment.json";

        public event Action<EquipmentData> OnEquipmentChanged;
        
        [ContextMenu("ClearEquipment")]
        private void ClearInventory()
        {
            _saveData = new EquipmentData();
            OnEquipmentChanged?.Invoke(_saveData);
            Save();
        }
        
        public void AddEquipment(EquipmentItem item)
        {
            EquipmentItem currentEquip = null;
            var equipmentType = item.GetEquipmentType();
            switch (equipmentType)
            {
                case EquipmentType.kevlar:
                    currentEquip = _saveData.kevlar;
                    _saveData.kevlar = item;
                    break;
                case EquipmentType.backpack:
                    currentEquip = _saveData.backpack;
                    _saveData.backpack = item;
                    break;
                case EquipmentType.weapon:
                    if (_saveData.firstWeapon == null)
                    {
                        currentEquip = _saveData.firstWeapon;
                        _saveData.firstWeapon = (Weapon) item;
                    }
                    else
                    {
                        currentEquip = _saveData.secondWeapon;
                        _saveData.secondWeapon = (Weapon) item;
                    }
                    break;
            }

            Inventory.Instance.DeleteItem(item);
            if (currentEquip != null)
            {
                Inventory.Instance.AddItem(currentEquip);
            }
            
            OnEquipmentChanged?.Invoke(_saveData);
        }

        public void RemoveEquipment(EquipmentItem item)
        {
            var equipmentType = item.GetEquipmentType();
            switch (equipmentType)
            {
                case EquipmentType.kevlar:
                    _saveData.kevlar = default;
                    break;
                case EquipmentType.backpack:
                    _saveData.backpack = default;
                    break;
                case EquipmentType.weapon:
                    if (_saveData.firstWeapon == item)
                        _saveData.firstWeapon = default;
                    else
                        _saveData.secondWeapon = default;
                    break;
            }
            
            Inventory.Instance.AddItem(item);
            
            OnEquipmentChanged?.Invoke(_saveData);
        }
        
        public EquipmentData GetInventory() => _saveData;
        
        protected override void Load()
        {
            base.Load();
            if (_saveData == null)
            {
                _saveData = new EquipmentData();
                Save();
            }
            
            OnEquipmentChanged?.Invoke(_saveData);
        }
        
        protected override void UpdatePath()
        {
            _secondPath = EQUIPMENT_JSON_PATH;
            base.UpdatePath();
        }
    }
    
    
    [Serializable]
    public class EquipmentData
    {
        public EquipmentItem kevlar;
        public EquipmentItem backpack;
        public Weapon firstWeapon;
        public Weapon secondWeapon;
    }

public enum EquipmentType
    {
        backpack,
        kevlar,
        weapon
    }