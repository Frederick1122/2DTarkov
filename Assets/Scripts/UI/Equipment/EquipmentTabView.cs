using System;
using Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentTabView : MonoBehaviour
{
    public event Action<EquipmentTabView> OnContainerClick;
    public event Action<IEquip> OnRemoveButtonClick;

    [SerializeField] private Image _icon;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _ammoDescription;
    [SerializeField] private TMP_Text _ammoQuantity;

    [SerializeField] private Button _containerButton;
    [SerializeField] private Button _removeButton;

    private IEquip _equipmentItem;

    public IEquip GetItem()
    {
        return _equipmentItem;
    }
    
    private void OnEnable()
    {
        _containerButton.onClick.AddListener(() => OnContainerClick?.Invoke(this));
        _removeButton.onClick.AddListener(() => OnRemoveButtonClick?.Invoke(_equipmentItem));
    }

    private void OnDisable()
    {
        _containerButton.onClick.RemoveAllListeners();
        _removeButton.onClick.RemoveAllListeners();
    }

    public void ChangeView(WeaponContainerData data)
    {
        data ??= new WeaponContainerData();

        if (data.equipmentItem != null)
        {
            _containerButton.gameObject.SetActive(true);
            _equipmentItem = data.equipmentItem;
        }
        else
        {
            _containerButton.gameObject.SetActive(false);
        }

        _name.text = data.itemName;
        _description.text = data.description;
        _ammoDescription.text = data.ammoDescription;

        _ammoQuantity.text = data.maxAmmoInMagazine > 0 ? $"{data.maxAmmoInMagazine} / {data.ammoInMagazine}" : "";

        if (data.icon == null)
        {
            _icon.gameObject.SetActive(false);
        }
        else
        {
            _icon.gameObject.SetActive(true);
            _icon.sprite = data.icon;
        }
    }

    public void Refresh()
    {
        _removeButton.gameObject.SetActive(false);
    }

    public void OpenActionButton()
    {
        _removeButton.gameObject.SetActive(true);
    }
}

public class WeaponContainerData
{
    public IEquip equipmentItem = default;
    public Sprite icon = null;
    public string itemName = "";
    public string description = "";
    public string ammoDescription = "";
    public int maxAmmoInMagazine = 0;
    public int ammoInMagazine = 0;
}