using System;
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

    private void UpdateView( EquipmentData equipmentData )
    {
        _equipmentPanelView.Refresh();

        var data = GenerateNewWindowData(equipmentData);

        _equipmentPanelView.ChangeViews(data);
    }

    private void ClickOnContainer( WeaponWindowView weaponWindowView )
    {
        _equipmentPanelView.Refresh();
        weaponWindowView.OpenActionButton();
    }

    private void ClickOnRemoveButton( EquipmentItem equipmentItem )
    {
        Equipment.Instance.RemoveEquipment(equipmentItem);
        _equipmentPanelView.Refresh();
    }

    private EquipmentWindowData GenerateNewWindowData( EquipmentData equipmentData )
    {
        var kevlarData = new WeaponContainerData();
        var backpackData = new WeaponContainerData();
        var firstWeaponData = new WeaponContainerData();
        var secondWeaponData = new WeaponContainerData();

        if( equipmentData.kevlar != null )
        {
            kevlarData = new WeaponContainerData
            {
                equipmentItem = equipmentData.kevlar,
                itemName = equipmentData.kevlar.itemName,
                icon = equipmentData.kevlar.icon,
                description = equipmentData.kevlar.description,
                ammoDescription = ""
            };
        }

        if( equipmentData.backpack != null )
        {
            backpackData = new WeaponContainerData
            {
                equipmentItem = equipmentData.backpack,
                icon = equipmentData.backpack.icon,
                itemName = equipmentData.backpack.itemName,
                description = equipmentData.backpack.description,
                ammoDescription = ""
            };
        }

        if( equipmentData.firstWeapon != null )
        {
            firstWeaponData = new WeaponContainerData
            {
                equipmentItem = equipmentData.firstWeapon,
                icon = equipmentData.firstWeapon.icon,
                itemName = equipmentData.firstWeapon.itemName,
                description = equipmentData.firstWeapon.description,
                ammoDescription = equipmentData.firstWeapon.bullet.itemName,
                maxAmmoInMagazine = equipmentData.firstWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipmentData.firstWeapon.maxAmmoInMagazine
            };
        }

        if( equipmentData.secondWeapon != null )
        {
            secondWeaponData = new WeaponContainerData
            {
                equipmentItem = equipmentData.secondWeapon,
                icon = equipmentData.secondWeapon.icon,
                itemName = equipmentData.secondWeapon.itemName,
                description = equipmentData.secondWeapon.description,
                ammoDescription = equipmentData.secondWeapon.bullet.itemName,
                maxAmmoInMagazine = equipmentData.secondWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipmentData.secondWeapon.maxAmmoInMagazine
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
