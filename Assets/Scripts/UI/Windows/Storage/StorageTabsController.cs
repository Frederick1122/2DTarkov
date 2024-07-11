using System;
using Base.MVC;

public class StorageTabsController : UIController
{
    public event Action OnClickEquipmentButton;
    public event Action OnClickStorageButton;
    
    public override void Init()
    {
        base.Init();

        GetView<StorageTabsView>().OnClickEquipmentButton += OnClickEquipmentButton;
        GetView<StorageTabsView>().OnClickStorageButton += OnClickStorageButton;
    }

    protected override UIModel GetViewData()
    {
        return new UIModel();
    }

    private void OnDestroy()
    {
        GetView<StorageTabsView>().OnClickEquipmentButton -= OnClickEquipmentButton;
        GetView<StorageTabsView>().OnClickStorageButton -= OnClickStorageButton;
    }
}
