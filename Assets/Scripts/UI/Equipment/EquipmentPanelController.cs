using System;
using Base;
using UnityEngine;

public class EquipmentPanelController : MonoBehaviour
{
    [SerializeField] private EquipmentPanelView _equipmentPanelView;

    private void OnDisable()
    {
        Equipment.Instance.OnEquipmentChanged -= UpdateView;
        _equipmentPanelView.OnContainerClick -= ClickOnContainer;
        _equipmentPanelView.OnRemoveButtonClick -= ClickOnRemoveButton;
    }

    private void Start()
    {
        _equipmentPanelView.ChangeViews();

        Equipment.Instance.OnEquipmentChanged += UpdateView;
        _equipmentPanelView.OnContainerClick += ClickOnContainer;
        _equipmentPanelView.OnRemoveButtonClick += ClickOnRemoveButton;
    }

    private void UpdateView(EquipmentData equipmentData)
    {
        _equipmentPanelView.Refresh();

        var data = GenerateNewWindowData(equipmentData);

        _equipmentPanelView.ChangeViews(data);
    }

    private void ClickOnContainer(EquipmentTabView equipmentTabView)
    {
        _equipmentPanelView.Refresh();
        equipmentTabView.OpenActionButton();
    }

    private void ClickOnRemoveButton(IEquip equipmentItem)
    {
        Equipment.Instance.RemoveEquipment(equipmentItem);
        _equipmentPanelView.Refresh();
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
                ammoInMagazine = firstWeapon.maxAmmoInMagazine
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
                ammoInMagazine = secondWeapon.maxAmmoInMagazine
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