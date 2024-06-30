using System;
using Base.MVC;
using UnityEngine;
using UnityEngine.UI;

public class StorageTabsView : UIView//<StorageTabsModel>
{
    public event Action OnClickEquipmentButton;
    public event Action OnClickStorageButton;
    
    [SerializeField] private Button _equipmentButton;
    [SerializeField] private Button _storageButton;

    public override void Init(UIModel uiModel)
    {
        _equipmentButton.onClick.AddListener(() => OnClickEquipmentButton.Invoke());
        _storageButton.onClick.AddListener(() => OnClickStorageButton.Invoke());
        base.Init(uiModel);
    }

    public override void Terminate()
    {
        _equipmentButton.onClick.RemoveAllListeners();
        _storageButton.onClick.RemoveAllListeners();
        base.Terminate();
    }
}

public class StorageTabsModel : UIModel {}