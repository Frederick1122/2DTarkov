using System;
using Base;
using UnityEngine;

public class EquipmentPanelController : UIController<EquipmentPanelView>
{
    public event Action<IEquip> OnContainerClick;
    public event Action OnRemoveButtonClick;

    [SerializeField] private InventoryWindowController _inventoryWindowController;

    private void OnEnable()
    {
        _view.OnContainerClick += ClickOnContainer;
        _view.OnRemoveButtonClick += ClickOnRemoveButton;
        _view.OnRemoveButtonClick += ClickOnRemoveButton;

        if (_inventoryWindowController != null)
        {
            _inventoryWindowController.OnClickCell += _view.Refresh;
        }
    }

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged -= UpdateView;
        _view.OnContainerClick -= ClickOnContainer;
        _view.OnRemoveButtonClick -= ClickOnRemoveButton;

        if (_inventoryWindowController != null)
        {
            _inventoryWindowController.OnClickCell -= _view.Refresh;
        }
    }

    private void Start()
    {
        _view.ChangeViews();

        Equipment.Instance.OnEquipmentChanged += UpdateView;

        UpdateView(Equipment.Instance.GetEquipment());
    }

    private void UpdateView(EquipmentData equipmentData)
    {
        _view.Refresh();

        var data = GenerateNewWindowData(equipmentData);

        _view.ChangeViews(data);
    }

    private void ClickOnContainer(EquipmentTabView equipmentTabView)
    {
        _view.Refresh();
        equipmentTabView.OpenActionButton();
        OnContainerClick?.Invoke(equipmentTabView.GetItem());
    }

    private void ClickOnRemoveButton(IEquip equipmentItem)
    {
        Equipment.Instance.RemoveEquipment(equipmentItem);
        _view.Refresh();
        OnRemoveButtonClick?.Invoke();
    }

    private EquipmentWindowData GenerateNewWindowData(EquipmentData equipmentData)
    {
        var kevlarData = new WeaponContainerData();
        var backpackData = new WeaponContainerData();
        var firstWeaponData = new WeaponContainerData();
        var secondWeaponData = new WeaponContainerData();

        var kevlar = (Weapon) equipmentData.GetEquipment(EquipmentType.kevlar);
        var backpack = (Weapon) equipmentData.GetEquipment(EquipmentType.backpack);
        var firstWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.firstWeapon);
        var secondWeapon = (Weapon) equipmentData.GetEquipment(EquipmentType.secondWeapon);

        if (kevlar != null)
        {
            kevlarData = new WeaponContainerData
            {
                equipmentItem = kevlar,
                itemName = kevlar.itemName,
                icon = kevlar.icon,
                description = kevlar.description,
                ammoDescription = ""
            };
        }

        if (backpack != null)
        {
            backpackData = new WeaponContainerData
            {
                equipmentItem = backpack,
                icon = backpack.icon,
                itemName = backpack.itemName,
                description = backpack.description,
                ammoDescription = ""
            };
        }

        if (firstWeapon != null)
        {
            firstWeaponData = new WeaponContainerData
            {
                equipmentItem = firstWeapon,
                icon = firstWeapon.icon,
                itemName = firstWeapon.itemName,
                description = firstWeapon.description,
                ammoDescription = firstWeapon.bullet.itemName,
                maxAmmoInMagazine = firstWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipmentData.firstWeaponAmmoInMagazine
            };
        }

        if (secondWeapon != null)
        {
            secondWeaponData = new WeaponContainerData
            {
                equipmentItem = secondWeapon,
                icon = secondWeapon.icon,
                itemName = secondWeapon.itemName,
                description = secondWeapon.description,
                ammoDescription = secondWeapon.bullet.itemName,
                maxAmmoInMagazine = secondWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipmentData.secondWeaponAmmoInMagazine
            };
        }

        var data = new EquipmentWindowData
        {
            kevlarContainerData = kevlarData,
            backpackContainerData = backpackData,
            firstWeaponContainerData = firstWeaponData,
            secondWeaponContainerData = secondWeaponData,
        };
        return data;
    }
}