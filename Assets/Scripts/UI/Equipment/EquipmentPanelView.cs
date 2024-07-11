using System;
using Base;
using Base.MVC;
using UnityEngine;

public class EquipmentPanelView : UIView//<EquipmentWindowModel>
{
    public event Action<EquipmentTabView> OnContainerClick;
    public event Action<IEquip> OnRemoveButtonClick;

    [SerializeField] private EquipmentTabView _kevlarContainer;
    [SerializeField] private EquipmentTabView _backpackContainer;
    [SerializeField] private EquipmentTabView _weaponContainer;
    [SerializeField] private EquipmentTabView _weapon2Container;
    
    public override void Init(UIModel uiModel)
    {
        _kevlarContainer.OnContainerClick += OnContainerClick;
        _backpackContainer.OnContainerClick += OnContainerClick;
        _weaponContainer.OnContainerClick += OnContainerClick;
        _weapon2Container.OnContainerClick += OnContainerClick;
        
        _kevlarContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _backpackContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _weaponContainer.OnRemoveButtonClick += OnRemoveButtonClick;
        _weapon2Container.OnRemoveButtonClick += OnRemoveButtonClick;
        base.Init(uiModel);
    }

    public override void Terminate()
    {
        _kevlarContainer.OnContainerClick -= OnContainerClick;
        _backpackContainer.OnContainerClick -= OnContainerClick;
        _weaponContainer.OnContainerClick -= OnContainerClick;
        _weapon2Container.OnContainerClick -= OnContainerClick;
        
        _kevlarContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _backpackContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _weaponContainer.OnRemoveButtonClick -= OnRemoveButtonClick;
        _weapon2Container.OnRemoveButtonClick -= OnRemoveButtonClick;
        base.Terminate();
    }

    public override void UpdateView(UIModel uiModel)
    {
        var castData = (EquipmentWindowModel) uiModel;
        
        _kevlarContainer.ChangeView(castData.kevlarContainerData);
        _backpackContainer.ChangeView(castData.backpackContainerData);
        _weaponContainer.ChangeView(castData.firstWeaponContainerData);
        _weapon2Container.ChangeView(castData.secondWeaponContainerData);
        base.UpdateView(uiModel);
    }

    public void Refresh()
    {
        _kevlarContainer.Refresh();
        _backpackContainer.Refresh();
        _weaponContainer.Refresh();
        _weapon2Container.Refresh();
    }
}

public class EquipmentWindowModel : UIModel
{
    public WeaponContainerData kevlarContainerData;
    public WeaponContainerData backpackContainerData;
    public WeaponContainerData firstWeaponContainerData;
    public WeaponContainerData secondWeaponContainerData;

    public EquipmentWindowModel(WeaponContainerData kevlarContainerData = null,
        WeaponContainerData backpackContainerData = null, WeaponContainerData firstWeaponContainerData = null,
        WeaponContainerData secondWeaponContainerData = null)
    {
        this.kevlarContainerData = kevlarContainerData ?? new WeaponContainerData();
        this.backpackContainerData = backpackContainerData ?? new WeaponContainerData();
        this.firstWeaponContainerData = firstWeaponContainerData ?? new WeaponContainerData();
        this.secondWeaponContainerData = secondWeaponContainerData ?? new WeaponContainerData();
    }
}