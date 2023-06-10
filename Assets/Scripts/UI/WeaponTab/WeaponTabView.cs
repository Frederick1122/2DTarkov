using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTabView : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _ammo;
    [SerializeField] private TMP_Text _reserve;

    [SerializeField] private Button _reloadButton;
    [SerializeField] private Button _swipeWeaponButton;

    public event Action onClickReload;
    public event Action onClickSwipeWeapon;

    private void OnEnable()
    {
        _reloadButton.onClick.AddListener(() => onClickReload?.Invoke());
        _swipeWeaponButton.onClick.AddListener(() => onClickSwipeWeapon?.Invoke());
    }

    private void OnDisable()
    {
        _reloadButton.onClick.RemoveAllListeners();
        _swipeWeaponButton.onClick.RemoveAllListeners();
    }

    public void UpdateView(WeaponTabModel weaponTabModel)
    {
        if (weaponTabModel == null)
        {
            _icon.gameObject.SetActive(false);
            _name.text = "Unarmed";
            _ammo.text = "";
            _reserve.text = "";
            return;
        }

        if (!_icon.gameObject.activeSelf)
        {
            _icon.gameObject.SetActive(true);
        }
        
        if (weaponTabModel.icon != null)
        {
            _icon.sprite = weaponTabModel.icon;
        }

        if (weaponTabModel.name != "")
        {
            _name.text = weaponTabModel.name;
        }

        if (weaponTabModel.maxAmmo == 0)
        {
            _ammo.text = "";
        } 
        else if (weaponTabModel.maxAmmo > 0)
        {
            _ammo.text = $"Ammo: {weaponTabModel.maxAmmo} / {weaponTabModel.currentAmmo}";
        }
        
        _reserve.text = weaponTabModel.reserve >= 0 ? $"Reserve: {weaponTabModel.reserve}" : "";
    }
}

public class WeaponTabModel
{
    public Sprite icon;
    public string name;
    public int maxAmmo;
    public int currentAmmo;
    public int reserve;

    public WeaponTabModel (Sprite icon = null, string name = "", int maxAmmo = -1, int currentAmmo = -1, int reserve = -1)
    {
        this.icon = icon;
        this.name = name;
        this.maxAmmo = maxAmmo;
        this.currentAmmo = currentAmmo;
        this.reserve = reserve;
    }
}
