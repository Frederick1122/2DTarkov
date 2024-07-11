using System;
using Base.MVC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponTabView : UIView
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _ammo;
    [SerializeField] private TMP_Text _reserve;

    [SerializeField] private Button _reloadButton;
    [SerializeField] private Button _swipeWeaponButton;

    public event Action OnClickReload;
    public event Action OnClickSwipeWeapon;

    public override void Init(UIModel uiModel)
    {
        _reloadButton.onClick.AddListener(() => OnClickReload?.Invoke());
        _swipeWeaponButton.onClick.AddListener(() => OnClickSwipeWeapon?.Invoke());
        base.Init(uiModel);
    }

    public override void Terminate()
    {
        _reloadButton.onClick.RemoveAllListeners();
        _swipeWeaponButton.onClick.RemoveAllListeners();
        base.Terminate();
    }

    public override void UpdateView(UIModel uiModel)
    {
        var weaponTabModel = (WeaponTabModel) uiModel;
        
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
        
        base.UpdateView(uiModel);
    }
}

public class WeaponTabModel : UIModel
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
