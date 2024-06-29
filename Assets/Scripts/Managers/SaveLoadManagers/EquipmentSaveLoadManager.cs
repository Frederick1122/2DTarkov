using System;
using Base;
using ConfigScripts;
using Managers.Libraries;
using UnityEngine;

namespace Managers.SaveLoadManagers
{
    public class EquipmentSaveLoadManager : SaveLoadManager<EquipmentData, EquipmentSaveLoadManager>
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

        public EquipmentData GetEquipment() => _saveData;
    
        public void AddEquipment(IEquip item)
        {
            var equipmentType = item.GetEquipmentType();
            var currentEquip = _saveData.GetEquipment(equipmentType);
            if (currentEquip != null)
            {
                RemoveEquipment(currentEquip);    
            }
        
            InventorySaveLoadManager.Instance.DeleteItem((ItemConfig) item);
            _saveData.SetEquipment(item, equipmentType);

            OnEquipmentChanged?.Invoke(_saveData);
        }

        public void RemoveEquipment(IEquip item)
        {
            var equipmentType = item.GetEquipmentType();
            var ammoInMagazine = _saveData.GetAmmoInMagazine(equipmentType);
            if (ammoInMagazine > 0)
            {
                var ammoItem = ((WeaponConfig) item).bulletConfig;
                InventorySaveLoadManager.Instance.AddItem(ammoItem, ammoInMagazine);
            }
        
            InventorySaveLoadManager.Instance.AddItem((ItemConfig) item);

            _saveData.SetEquipment(default, item.GetEquipmentType());
            OnEquipmentChanged?.Invoke(_saveData);
        }

        public int GetAmmoInMagazine(WeaponConfig weaponConfig)
        {
            if (weaponConfig.GetEquipmentType() == EquipmentType.firstWeapon)
            {
                return _saveData.firstWeaponAmmoInMagazine;
            }

            return _saveData.secondWeaponAmmoInMagazine;
        }

        public void SetAmmoInMagazine(WeaponConfig weaponConfig, int ammoInMagazine)
        {
            if (weaponConfig.GetEquipmentType() == EquipmentType.firstWeapon)
            {
                _saveData.firstWeaponAmmoInMagazine = ammoInMagazine;
            }
            else
            {
                _saveData.secondWeaponAmmoInMagazine = ammoInMagazine;
            }
        
            Save();
            OnEquipmentChanged?.Invoke(_saveData);
        }

        public void ReloadWeapon()
        {
            ReloadWeapon(null);
        }
    
        public void ReloadWeapon(WeaponConfig currentWeaponConfig)
        {
            if (currentWeaponConfig == null)
            {
                var currentType = _saveData.isSecondWeapon ? EquipmentType.secondWeapon : EquipmentType.firstWeapon;
                currentWeaponConfig = (WeaponConfig) _saveData.GetEquipment(currentType);       
                if (currentWeaponConfig == null)
                    return;
            }

            if (_saveData.isSecondWeapon && currentWeaponConfig.configPath != _saveData.secondWeaponConfigKey
                || !_saveData.isSecondWeapon && currentWeaponConfig.configPath != _saveData.firstWeaponConfigKey)
            {
                return;
            }
            
            var maxAmmoInMagazine = currentWeaponConfig.maxAmmoInMagazine;
            var ammoInMagazine = GetAmmoInMagazine(currentWeaponConfig);
        
            var reserve = InventorySaveLoadManager.Instance.GetItemCount(currentWeaponConfig.bulletConfig);

            if (reserve > 0 && maxAmmoInMagazine != ammoInMagazine)
            {
                var addedAmmo = Mathf.Clamp(maxAmmoInMagazine - ammoInMagazine, 0, reserve);
                InventorySaveLoadManager.Instance.DeleteItem(currentWeaponConfig.bulletConfig, addedAmmo);
                SetAmmoInMagazine(currentWeaponConfig, ammoInMagazine + addedAmmo);
            }
        }

        public void SwipeWeapon()
        {
            _saveData.isSecondWeapon = !_saveData.isSecondWeapon;
            Save();
            OnEquipmentChanged?.Invoke(_saveData);
        }

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
        public string kevlarConfigKey;
        public string backpackConfigKey;
        public string firstWeaponConfigKey;
        public string secondWeaponConfigKey;

        public int firstWeaponAmmoInMagazine;
        public int secondWeaponAmmoInMagazine;
        public bool isSecondWeapon;
    
        private WeaponConfig _kevlar;
        private WeaponConfig _backpack;
        private WeaponConfig _firstWeaponConfig;
        private WeaponConfig _secondWeaponConfig;

        public void SetEquipment(IEquip equipmentItem, EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.kevlar:
                    _kevlar = (WeaponConfig) equipmentItem;
                    kevlarConfigKey = _kevlar == default ? "" : _kevlar.configKey;
                    break;
                case EquipmentType.backpack:
                    _backpack = (WeaponConfig) equipmentItem;
                    backpackConfigKey = _backpack == default ? "" : _backpack.configKey;
                    break;
                case EquipmentType.firstWeapon:
                    _firstWeaponConfig = (WeaponConfig) equipmentItem;
                    firstWeaponConfigKey = _firstWeaponConfig == default ? "" : _firstWeaponConfig.configKey;
                    firstWeaponAmmoInMagazine = 0;
                    break;
                case EquipmentType.secondWeapon:
                    _secondWeaponConfig = (WeaponConfig) equipmentItem;
                    secondWeaponConfigKey = _secondWeaponConfig == default ? "" : _secondWeaponConfig.configKey;
                    secondWeaponAmmoInMagazine = 0;
                    break;
            }
        }

        public IEquip GetEquipment(EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.kevlar:
                    return GetConfig(kevlarConfigKey, _kevlar) as WeaponConfig;
                case EquipmentType.backpack:
                    return GetConfig(backpackConfigKey, _backpack) as WeaponConfig;
                case EquipmentType.firstWeapon:
                    return GetConfig(firstWeaponConfigKey, _firstWeaponConfig) as WeaponConfig;
                case EquipmentType.secondWeapon:
                    return GetConfig(secondWeaponConfigKey, _secondWeaponConfig) as WeaponConfig;
            }

            return null;
        }

        public int GetAmmoInMagazine(EquipmentType equipmentType)
        {
            if (equipmentType == EquipmentType.firstWeapon)
            {
                return firstWeaponAmmoInMagazine;
            }
            else if (equipmentType == EquipmentType.secondWeapon)
            {
                return secondWeaponAmmoInMagazine;
            }

            return 0;
        }

        private ItemConfig GetConfig(string configKey, ItemConfig loadedItemConfig)
        {
            if (configKey == "")
                return null;
                    
            if (loadedItemConfig == null || loadedItemConfig == default)
                return ItemLibrary.Instance.GetConfig(configKey) as WeaponConfig;

            return loadedItemConfig;
        }
    }

    public enum EquipmentType
    {
        backpack,
        kevlar,
        firstWeapon,
        secondWeapon,
        melee
    }
}