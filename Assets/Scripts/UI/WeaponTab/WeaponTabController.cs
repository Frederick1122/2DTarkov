using ConfigScripts;
using Managers.SaveLoadManagers;
using UnityEngine;

public class WeaponTabController : MonoBehaviour
{
    [SerializeField] private WeaponTabView _weaponTabView;

    private WeaponConfig _currentWeaponConfig;

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
        _currentWeaponConfig = (WeaponConfig) equipmentData.GetEquipment(currentType);
        var currentAmmo = equipmentData.isSecondWeapon
            ? equipmentData.secondWeaponAmmoInMagazine
            : equipmentData.firstWeaponAmmoInMagazine;
        if (_currentWeaponConfig != null)
        {
            var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeaponConfig.bulletConfig);
            var weaponTabModel = new WeaponTabModel(_currentWeaponConfig.icon,
                _currentWeaponConfig.itemName,
                _currentWeaponConfig.maxAmmoInMagazine,
                currentAmmo,
                reserve);

            _weaponTabView.UpdateView(weaponTabModel);
        }
        else
        {
            _weaponTabView.UpdateView(null);
        }
    }

    private void UpdateReverse(ItemConfig itemConfig, int count, InventoryType inventoryType)
    {
        if(_currentWeaponConfig == null || itemConfig != _currentWeaponConfig.bulletConfig)
            return;
        
        var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeaponConfig.bulletConfig);

        var weaponTabModel = new WeaponTabModel(reserve: reserve);
        
        _weaponTabView.UpdateView(weaponTabModel);
    }
    
    private void ReloadCurrentWeapon()
    {
        
    }

    private void SwipeWeapon()
    {
        EquipmentSaveLoadManager.Instance.SwipeWeapon();
    }
}