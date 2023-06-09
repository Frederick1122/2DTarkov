using System;
using Base;
using UnityEngine;

public class Equipment : SaveLoadManager<EquipmentData, Equipment>
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

    public void AddEquipment(IEquip item)
    {
        var equipmentType = item.GetEquipmentType();

        var currentEquip = _saveData.GetEquipment(equipmentType);
        _saveData.SetEquipment(item, equipmentType);

        Inventory.Instance.DeleteItem((Item) item);
        
        if (currentEquip != null)
        {
            Inventory.Instance.AddItem((Item) currentEquip);
        }

        OnEquipmentChanged?.Invoke(_saveData);
    }

    public void RemoveEquipment(IEquip item)
    {
        _saveData.SetEquipment(default, item.GetEquipmentType());

        Inventory.Instance.AddItem((Item) item);

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
    public string kevlarConfigPath;
    public string backpackConfigPath;
    public string firstWeaponConfigPath;
    public string secondWeaponConfigPath;

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
                break;
            case EquipmentType.secondWeapon:
                _secondWeapon = (Weapon) equipmentItem;
                secondWeaponConfigPath = _secondWeapon == default ? "" : _secondWeapon.configPath;
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
}

public enum EquipmentType
{
    backpack,
    kevlar,
    firstWeapon,
    secondWeapon
}