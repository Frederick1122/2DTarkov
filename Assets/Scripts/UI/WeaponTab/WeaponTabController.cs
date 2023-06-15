using UnityEngine;

public class WeaponTabController : MonoBehaviour
{
    [SerializeField] private WeaponTabView _weaponTabView;

    private Weapon _currentWeapon;

    private void Start()
    {
        Equipment.Instance.OnEquipmentChanged += UpdateView;
        Inventory.Instance.OnInventoryAdded += UpdateReverse;
        Inventory.Instance.OnInventoryDeleted += UpdateReverse;

        _weaponTabView.onClickReload += ReloadCurrentWeapon;
        _weaponTabView.onClickSwipeWeapon += SwipeWeapon;
        
        UpdateView(Equipment.Instance.GetEquipment());
    }

    private void OnDisable()
    {
        _weaponTabView.onClickReload -= ReloadCurrentWeapon;
        _weaponTabView.onClickSwipeWeapon -= SwipeWeapon;
        
        Equipment.Instance.OnEquipmentChanged -= UpdateView;
        Inventory.Instance.OnInventoryAdded -= UpdateReverse;
        Inventory.Instance.OnInventoryDeleted -= UpdateReverse;
    }

    private void UpdateView(EquipmentData equipmentData)
    {
        var currentType = equipmentData.isSecondWeapon ? EquipmentType.secondWeapon : EquipmentType.firstWeapon;
        _currentWeapon = (Weapon) equipmentData.GetEquipment(currentType);
        var currentAmmo = equipmentData.isSecondWeapon
            ? equipmentData.secondWeaponAmmoInMagazine
            : equipmentData.firstWeaponAmmoInMagazine;
        if (_currentWeapon != null)
        {
            var reserve = Inventory.Instance.GetItemCount(_currentWeapon.bullet);
            var weaponTabModel = new WeaponTabModel(_currentWeapon.icon,
                _currentWeapon.itemName,
                _currentWeapon.maxAmmoInMagazine,
                currentAmmo,
                reserve);

            _weaponTabView.UpdateView(weaponTabModel);
        }
        else
        {
            _weaponTabView.UpdateView(null);
        }
    }

    private void UpdateReverse(Item item, int count)
    {
        if(_currentWeapon == null || item != _currentWeapon.bullet)
            return;
        
        var reserve = Inventory.Instance.GetItemCount(_currentWeapon.bullet);

        var weaponTabModel = new WeaponTabModel(reserve: reserve);
        
        _weaponTabView.UpdateView(weaponTabModel);
    }
    
    private void ReloadCurrentWeapon()
    {
        if (_currentWeapon == null)
            return;
        
        var maxAmmoInMagazine = _currentWeapon.maxAmmoInMagazine;
        var ammoInMagazine = Equipment.Instance.GetAmmoInMagazine(_currentWeapon);
        
        var reserve = Inventory.Instance.GetItemCount(_currentWeapon.bullet);

        if (reserve > 0 && maxAmmoInMagazine != ammoInMagazine)
        {
            var addedAmmo = Mathf.Clamp(maxAmmoInMagazine - ammoInMagazine, 0, reserve);
            Inventory.Instance.DeleteItem(_currentWeapon.bullet, addedAmmo);
            Equipment.Instance.SetAmmoInMagazine( _currentWeapon, ammoInMagazine + addedAmmo);
        }
    }

    private void SwipeWeapon()
    {
        Equipment.Instance.SwipeWeapon();
    }
}