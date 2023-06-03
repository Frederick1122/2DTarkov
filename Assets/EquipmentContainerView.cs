using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentContainerView : MonoBehaviour
{
    public event Action OnContainerClick;
    public event Action OnRemoveButtonClick;
    
    [SerializeField] private Image _icon;
    
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _ammoDescription;
    [SerializeField] private TMP_Text _ammoQuantity;

    [SerializeField] private Button _containerButton;
    [SerializeField] private Button _removeButton;

    private void OnEnable()
    {
        _containerButton.onClick.AddListener(() => OnContainerClick?.Invoke());
        _removeButton.onClick.AddListener(() => OnRemoveButtonClick?.Invoke());
    }

    private void OnDisable()
    {
        _containerButton.onClick.RemoveAllListeners();
        _removeButton.onClick.RemoveAllListeners();
    }

    public void Init( EquipmentContainerData data )
    {
        _icon.sprite = data.icon;
        _name.text = data.name;
        _description.text = data.description;
        _ammoDescription.text = data.ammoDescription;
        _ammoQuantity.text = $"{data.maxAmmoInMagazine} / {data.ammoInMagazine}";
    }
}

public class EquipmentContainerData
{
    public Sprite icon;
    public string name;
    public string description;
    public string ammoDescription;
    public int maxAmmoInMagazine;
    public int ammoInMagazine;
}