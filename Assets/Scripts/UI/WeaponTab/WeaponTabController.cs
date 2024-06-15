using UnityEngine;

public class WeaponTabController : MonoBehaviour
{
    [SerializeField] private WeaponTabView _weaponTabView;

    private Weapon _currentWeapon;

    private void Start()
    {
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged += UpdateView;
        InventorySaveLoadManager.Instance.OnInventoryAdded += UpdateReverse;
        InventorySaveLoadManager.Instance.OnInventoryDeleted += UpdateReverse;

        _weaponTabView.onClickReload += ReloadCurrentWeapon;
        _weaponTabView.onClickSwipeWeapon += SwipeWeapon;
        
        UpdateView(EquipmentSaveLoadManager.Instance.GetEquipment());
    }

    private void OnDisable()
    {
        _weaponTabView.onClickReload -= ReloadCurrentWeapon;
        _weaponTabView.onClickSwipeWeapon -= SwipeWeapon;
        
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged -= UpdateView;
        InventorySaveLoadManager.Instance.OnInventoryAdded -= UpdateReverse;
        InventorySaveLoadManager.Instance.OnInventoryDeleted -= UpdateReverse;
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
            var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeapon.bullet);
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

    private void UpdateReverse(Item item, int count, InventoryType inventoryType)
    {
        if(_currentWeapon == null || item != _currentWeapon.bullet)
            return;
        
        var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeapon.bullet);

        var weaponTabModel = new WeaponTabModel(reserve: reserve);
        
        _weaponTabView.UpdateView(weaponTabModel);
    }
    
    private void ReloadCurrentWeapon()
    {
        if (_currentWeapon == null)
            return;
        
        var maxAmmoInMagazine = _currentWeapon.maxAmmoInMagazine;
        var ammoInMagazine = EquipmentSaveLoadManager.Instance.GetAmmoInMagazine(_currentWeapon);
        
        var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeapon.bullet);

        if (reserve > 0 && maxAmmoInMagazine != ammoInMagazine)
        {
            var addedAmmo = Mathf.Clamp(maxAmmoInMagazine - ammoInMagazine, 0, reserve);
            InventorySaveLoadManager.Instance.DeleteItem(_currentWeapon.bullet, addedAmmo);
            EquipmentSaveLoadManager.Instance.SetAmmoInMagazine( _currentWeapon, ammoInMagazine + addedAmmo);
        }
    }

    private void SwipeWeapon()
    {
        EquipmentSaveLoadManager.Instance.SwipeWeapon();
    }
}