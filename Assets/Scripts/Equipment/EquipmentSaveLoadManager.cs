using System;
using Base;
using UnityEngine;

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
        
        InventorySaveLoadManager.Instance.DeleteItem((Item) item);
        _saveData.SetEquipment(item, equipmentType);

        OnEquipmentChanged?.Invoke(_saveData);
    }

    public void RemoveEquipment(IEquip item)
    {
        var equipmentType = item.GetEquipmentType();
        var ammoInMagazine = _saveData.GetAmmoInMagazine(equipmentType);
        if (ammoInMagazine > 0)
        {
            var ammoItem = ((Weapon) item).bullet;
            InventorySaveLoadManager.Instance.AddItem(ammoItem, ammoInMagazine);
        }
        
        InventorySaveLoadManager.Instance.AddItem((Item) item);

        _saveData.SetEquipment(default, item.GetEquipmentType());
        OnEquipmentChanged?.Invoke(_saveData);
    }

    public int GetAmmoInMagazine(Weapon weapon)
    {
        if (weapon.GetEquipmentType() == EquipmentType.firstWeapon)
        {
            return _saveData.firstWeaponAmmoInMagazine;
        }

        return _saveData.secondWeaponAmmoInMagazine;
    }

    public void SetAmmoInMagazine(Weapon weapon, int ammoInMagazine)
    {
        if (weapon.GetEquipmentType() == EquipmentType.firstWeapon)
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
    public string kevlarConfigPath;
    public string backpackConfigPath;
    public string firstWeaponConfigPath;
    public string secondWeaponConfigPath;

    public int firstWeaponAmmoInMagazine;
    public int secondWeaponAmmoInMagazine;
    public bool isSecondWeapon;
    
    private Weapon _kevlar;
    private Weapon _backpack;
    private Weapon _firstWeapon;
    private Weapon _secondWeapon;

    public void SetEquipment(IEquip equipmentItem, EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.kevlar:
                _kevlar = (Weapon) equipmentItem;
                kevlarConfigPath = _kevlar == default ? "" : _kevlar.configPath;
                break;
            case EquipmentType.backpack:
                _backpack = (Weapon) equipmentItem;
                backpackConfigPath = _backpack == default ? "" : _backpack.configPath;
                break;
            case EquipmentType.firstWeapon:
                _firstWeapon = (Weapon) equipmentItem;
                firstWeaponConfigPath = _firstWeapon == default ? "" : _firstWeapon.configPath;
                firstWeaponAmmoInMagazine = 0;
                break;
            case EquipmentType.secondWeapon:
                _secondWeapon = (Weapon) equipmentItem;
                secondWeaponConfigPath = _secondWeapon == default ? "" : _secondWeapon.configPath;
                secondWeaponAmmoInMagazine = 0;
                break;
        }
    }

    public IEquip GetEquipment(EquipmentType equipmentType)
    {
        switch (equipmentType)
        {
            case EquipmentType.kevlar:
                if (_kevlar == null || _kevlar == default)
                    _kevlar = Resources.Load<Weapon>(kevlarConfigPath);

                return _kevlar;
            case EquipmentType.backpack:
                if (_backpack == null || _backpack == default)
                    _backpack = Resources.Load<Weapon>(backpackConfigPath);

                return _backpack;
            case EquipmentType.firstWeapon:
                if (_firstWeapon == null || _firstWeapon == default)
                    _firstWeapon = Resources.Load<Weapon>(firstWeaponConfigPath);

                return _firstWeapon;
            case EquipmentType.secondWeapon:
                if (_secondWeapon == null || _secondWeapon == default)
                    _secondWeapon = Resources.Load<Weapon>(secondWeaponConfigPath);

                return _secondWeapon;
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
}

public enum EquipmentType
{
    backpack,
    kevlar,
    firstWeapon,
    secondWeapon,
    knife
}