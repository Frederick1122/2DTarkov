using System;
using UnityEngine;

public class EquipmentPanelController : MonoBehaviour
{
    [SerializeField] private EquipmentPanelView _equipmentPanelView;

    private void OnEnable()
    {
        EquipmentManager.Instance.OnEquipmentChanged += UpdateView;
        _equipmentPanelView.OnContainerClick += ClickOnContainer;
        _equipmentPanelView.OnRemoveButtonClick += ClickOnRemoveButton;
    }

    private void OnDisable()
    {
        EquipmentManager.Instance.OnEquipmentChanged -= UpdateView;
        _equipmentPanelView.OnContainerClick -= ClickOnContainer;
        _equipmentPanelView.OnRemoveButtonClick -= ClickOnRemoveButton;
    }

    private void Start()
    {
        _equipmentPanelView.ChangeViews();
    }

    private void UpdateView( Equipment equipment )
    {
        _equipmentPanelView.Refresh();

        var data = GenerateNewWindowData(equipment);

        _equipmentPanelView.ChangeViews(data);
    }

    private void ClickOnContainer( WeaponWindowView weaponWindowView )
    {
        _equipmentPanelView.Refresh();
        weaponWindowView.OpenActionButton();
    }

    private void ClickOnRemoveButton( EquipmentItem equipmentItem )
    {
        EquipmentManager.Instance.RemoveEquipment(equipmentItem);
        _equipmentPanelView.Refresh();
    }

    private EquipmentWindowData GenerateNewWindowData( Equipment equipment )
    {
        var kevlarData = new WeaponContainerData();
        var backpackData = new WeaponContainerData();
        var firstWeaponData = new WeaponContainerData();
        var secondWeaponData = new WeaponContainerData();

        if( equipment.kevlar != null )
        {
            kevlarData = new WeaponContainerData
            {
                equipmentItem = equipment.kevlar,
                icon = equipment.kevlar.icon,
                description = equipment.kevlar.description,
                ammoDescription = ""
            };
        }

        if( equipment.backpack != null )
        {
            backpackData = new WeaponContainerData
            {
                equipmentItem = equipment.backpack,
                icon = equipment.backpack.icon,
                description = equipment.backpack.description,
                ammoDescription = ""
            };
        }

        if( equipment.firstWeapon != null )
        {
            firstWeaponData = new WeaponContainerData
            {
                equipmentItem = equipment.firstWeapon,
                icon = equipment.firstWeapon.icon,
                description = equipment.firstWeapon.description,
                ammoDescription = equipment.firstWeapon.bullet.itemName,
                maxAmmoInMagazine = equipment.firstWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipment.firstWeapon.maxAmmoInMagazine
            };
        }

        if( equipment.secondWeapon != null )
        {
            secondWeaponData = new WeaponContainerData
            {
                equipmentItem = equipment.secondWeapon,
                icon = equipment.secondWeapon.icon,
                description = equipment.secondWeapon.description,
                ammoDescription = equipment.secondWeapon.bullet.itemName,
                maxAmmoInMagazine = equipment.secondWeapon.maxAmmoInMagazine,
                ammoInMagazine = equipment.secondWeapon.maxAmmoInMagazine
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
