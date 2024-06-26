﻿using System;
using Base;
using Base.MVC;
using ConfigScripts;
using Managers.SaveLoadManagers;
using UI.Windows.Inventory;
using UnityEngine;

public class EquipmentPanelController : UIController//<EquipmentPanelView, EquipmentWindowModel>
{
    public event Action<IEquip> OnContainerClick;
    public event Action OnRemoveButtonClick;

    [SerializeField] private InventoryWindowController _inventoryWindowController;

    private EquipmentData _data;

    private void OnEnable()
    {
        GetView<EquipmentPanelView>().OnContainerClick += ClickOnContainer;
        GetView<EquipmentPanelView>().OnRemoveButtonClick += ClickOnRemoveButton;

        if (_inventoryWindowController != null)
        {
            _inventoryWindowController.OnClickCell += GetView<EquipmentPanelView>().Refresh;
        }
    }

    private void OnDisable()
    {
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged -= UpdateView;
        GetView<EquipmentPanelView>().OnContainerClick -= ClickOnContainer;
        GetView<EquipmentPanelView>().OnRemoveButtonClick -= ClickOnRemoveButton;

        if (_inventoryWindowController != null)
        {
            _inventoryWindowController.OnClickCell -= GetView<EquipmentPanelView>().Refresh;
        }
    }

    private void Start()
    {
        _view.UpdateView(new EquipmentWindowModel());

        EquipmentSaveLoadManager.Instance.OnEquipmentChanged += UpdateView;

        UpdateView(EquipmentSaveLoadManager.Instance.GetEquipment());
    }

    private void UpdateView(EquipmentData equipmentData)
    {
        GetView<EquipmentPanelView>().Refresh();

        _data = equipmentData;

        UpdateView();
    }

    private void ClickOnContainer(EquipmentTabView equipmentTabView)
    {
        GetView<EquipmentPanelView>().Refresh();
        equipmentTabView.OpenActionButton();
        OnContainerClick?.Invoke(equipmentTabView.GetItem());
    }

    private void ClickOnRemoveButton(IEquip equipmentItem)
    {
        EquipmentSaveLoadManager.Instance.RemoveEquipment(equipmentItem);
        GetView<EquipmentPanelView>().Refresh();
        OnRemoveButtonClick?.Invoke();
    }

    private EquipmentWindowModel GenerateNewWindowData(EquipmentData equipmentData)
    {
        var kevlarData = new WeaponContainerData();
        var backpackData = new WeaponContainerData();
        var firstWeaponData = new WeaponContainerData();
        var secondWeaponData = new WeaponContainerData();

        var kevlar = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.kevlar);
        var backpack = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.backpack);
        var firstWeapon = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.firstWeapon);
        var secondWeapon = (WeaponConfig) equipmentData.GetEquipment(EquipmentType.secondWeapon);

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
                ammoDescription = firstWeapon.bulletConfig.itemName,
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
                ammoDescription = secondWeapon.bulletConfig.itemName,
                maxAmmoInMagazine = secondWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipmentData.secondWeaponAmmoInMagazine
            };
        }

        var data = new EquipmentWindowModel(kevlarData, backpackData, firstWeaponData, secondWeaponData);
        return data;
    }

    protected override UIModel GetViewData()
    {
        return GenerateNewWindowData(_data);
    }
}