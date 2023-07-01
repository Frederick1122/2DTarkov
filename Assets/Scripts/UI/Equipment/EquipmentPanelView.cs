using System;
using Base;
using UnityEngine;

public class EquipmentPanelView : UIView
{
    public event Action<EquipmentTabView> OnContainerClick;
    public event Action<IEquip> OnRemoveButtonClick;

    [SerializeField] private EquipmentTabView _kevlarContainer;
    [SerializeField] private EquipmentTabView _backpackContainer;
    [SerializeField] private EquipmentTabView _weaponContainer;
    [SerializeField] private EquipmentTabView _weapon2Container;

    private void OnEnable()
    {
        _kevlarContainer.OnContainerClick += OnContainerClick;
        _backpackContainer.OnContainerClick += OnContainerClick;
        _weaponContainer.OnContainerClick += OnContainerClick;
        _weapon2Container.OnContainerClick += OnContainerClick;
        
        _kevlarContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _backpackContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _weaponContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _weapon2Container.OnRemoveButtonClick += OnRemoveButtonClick;
    }

    private void OnDisable()
    {
        _kevlarContainer.OnContainerClick -= OnContainerClick;
        _backpackContainer.OnContainerClick -= OnContainerClick;
        _weaponContainer.OnContainerClick -= OnContainerClick;
        _weapon2Container.OnContainerClick -= OnContainerClick;
        
        _kevlarContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _backpackContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _weaponContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _weapon2Container.OnRemoveButtonClick -= OnRemoveButtonClick;
    }

    public void ChangeViews( EquipmentWindowData data = null)
    {
        data ??= new EquipmentWindowData();
        
        _kevlarContainer.ChangeView(data.kevlarContainerData);
        _backpackContainer.ChangeView(data.backpackContainerData);
        _weaponContainer.ChangeView(data.firstWeaponContainerData);
        _weapon2Container.ChangeView(data.secondWeaponContainerData);
    }

    public void Refresh()
    {
        _kevlarContainer.Refresh();
        _backpackContainer.Refresh();
        _weaponContainer.Refresh();
        _weapon2Container.Refresh();
    }
}

public class EquipmentWindowData
{
    public WeaponContainerData kevlarContainerData;
    public WeaponContainerData backpackContainerData;
    public WeaponContainerData firstWeaponContainerData;
    public WeaponContainerData secondWeaponContainerData;
}