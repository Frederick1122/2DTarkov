using UI;
using UnityEngine;

public class StorageWindowController : WindowController<StorageView, StorageModel>
{
    [SerializeField] private InventoryWindowController _inventoryWindowController;
    [SerializeField] private InventoryWindowController _storageController;
    [SerializeField] private StorageTabsController _storageTabsController;
    [SerializeField] private EquipmentPanelController _equipmentPanelController;

    private bool _isStorageState;
    public override void Init()
    {
        base.Init();
        _inventoryWindowController.OnClickCell += ClearOldStorageCell;
        _storageController.OnClickCell += ClearOldInventoryCell;

        _storageTabsController.OnClickEquipmentButton += OpenEquipmentTab;
        _storageTabsController.OnClickStorageButton += OpenStorageTab;
        
        _inventoryWindowController.Init(InventoryType.Inventory, true);
        _storageController.Init(InventoryType.Storage, true);
        _storageTabsController.Init();
        _equipmentPanelController.Init();
        
        OpenStorageTab();
    }

    public override void Terminate()
    {
        _inventoryWindowController.OnClickCell += ClearOldStorageCell;
        _storageController.OnClickCell += ClearOldInventoryCell;
        
        _storageTabsController.OnClickEquipmentButton -= OpenEquipmentTab;
        _storageTabsController.OnClickStorageButton -= OpenStorageTab;
        
        _inventoryWindowController.Terminate();
        _storageController.Terminate();
        _storageTabsController.Terminate();
        _equipmentPanelController.Terminate();
        base.Terminate();
    }

    public override void Show()
    {
        base.Show();
        _inventoryWindowController.Show();
        _storageController.Show();
        
        if(_isStorageState)
            OpenStorageTab();
        else
            OpenEquipmentTab();
    }
    
    public override void Hide()
    {
        base.Hide();
        _inventoryWindowController.Hide();
        _storageController.Hide();
    }

    private void ClearOldInventoryCell()
    {
        _inventoryWindowController.ClearCurrentCell();
    }
    
    private void ClearOldStorageCell()
    {
        _storageController.ClearCurrentCell();
    }

    private void OpenStorageTab()
    {
        _storageController.Show();
        _equipmentPanelController.Hide();

        _isStorageState = true;
    }
    
    private void OpenEquipmentTab()
    {
        _equipmentPanelController.Show();
        _storageController.Hide();

        _isStorageState = false;
    }
}