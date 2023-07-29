using System;

public class StorageTabsController : UIController<StorageTabsView, StorageTabsModel>
{
    public event Action OnClickEquipmentButton;
    public event Action OnClickStorageButton;
    
    public override void Init()
    {
        base.Init();
        _view.Init();

        _view.OnClickEquipmentButton += OnClickEquipmentButton;
        _view.OnClickStorageButton += OnClickStorageButton;
    }

    public override void Terminate()
    {
        _view.OnClickEquipmentButton -= OnClickEquipmentButton;
        _view.OnClickStorageButton -= OnClickStorageButton;
        
        base.Terminate();
    }
}
