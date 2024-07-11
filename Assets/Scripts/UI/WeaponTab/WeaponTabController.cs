using Base.MVC;
using ConfigScripts;
using Managers.SaveLoadManagers;

public class WeaponTabController : UIController
{
    private WeaponConfig _currentWeaponConfig;

    private EquipmentData _equipmentData;
    public override void Init()
    {
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged += SetNewEquipment;
        InventorySaveLoadManager.Instance.OnInventoryAdded += HandleinventoryUpdate;
        InventorySaveLoadManager.Instance.OnInventoryDeleted += HandleinventoryUpdate;

        GetView<WeaponTabView>().OnClickReload += ReloadCurrentWeapon;
        GetView<WeaponTabView>().OnClickSwipeWeapon += SwipeWeapon;
        
        SetNewEquipment(EquipmentSaveLoadManager.Instance.GetEquipment());
        base.Init();
    }

    public override void Terminate()
    {
        GetView<WeaponTabView>().OnClickReload -= ReloadCurrentWeapon;
        GetView<WeaponTabView>().OnClickSwipeWeapon -= SwipeWeapon;
        
        EquipmentSaveLoadManager.Instance.OnEquipmentChanged -= SetNewEquipment;
        InventorySaveLoadManager.Instance.OnInventoryAdded -= HandleinventoryUpdate;
        InventorySaveLoadManager.Instance.OnInventoryDeleted -= HandleinventoryUpdate;
        base.Terminate();
    }

    protected override UIModel GetViewData()
    {
        var currentType = _equipmentData.isSecondWeapon ? EquipmentType.secondWeapon : EquipmentType.firstWeapon;
        _currentWeaponConfig = (WeaponConfig) _equipmentData.GetEquipment(currentType);
        var currentAmmo = _equipmentData.isSecondWeapon
            ? _equipmentData.secondWeaponAmmoInMagazine
            : _equipmentData.firstWeaponAmmoInMagazine;
        if (_currentWeaponConfig != null)
        {
            var reserve = InventorySaveLoadManager.Instance.GetItemCount(_currentWeaponConfig.bulletConfig);
            var weaponTabModel = new WeaponTabModel(_currentWeaponConfig.icon,
                _currentWeaponConfig.itemName,
                _currentWeaponConfig.maxAmmoInMagazine,
                currentAmmo,
                reserve);

            return weaponTabModel;
        }
        
        return null;
    }

    private void HandleinventoryUpdate(ItemConfig config, int count, InventoryType type)
    {
        UpdateView();
    }

    private void SetNewEquipment(EquipmentData equipmentData)
    {
        _equipmentData = equipmentData;
        UpdateView();
    }
    
    private void ReloadCurrentWeapon()
    {
        
    }

    private void SwipeWeapon()
    {
        EquipmentSaveLoadManager.Instance.SwipeWeapon();
    }
}