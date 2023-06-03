using System;
using UnityEngine;

public class EquipmentPanelView : MonoBehaviour
{
    public event Action<WeaponWindowView> OnContainerClick;
    public event Action<EquipmentItem> OnRemoveButtonClick;

    [SerializeField] private WeaponWindowView _kevlarContainer;
    [SerializeField] private WeaponWindowView _backpackContainer;
    [SerializeField] private WeaponWindowView _weaponContainer;
    [SerializeField] private WeaponWindowView _weapon2Container;

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