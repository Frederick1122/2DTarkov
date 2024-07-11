using System;
using Base.MVC;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : WindowView //<MainMenuModel>
{
    [SerializeField] private Button _sortie;
    [SerializeField] private Button _storage;
    [SerializeField] private Button _merchants;

    public event Action OnClickSortie;
    public event Action OnClickStorage;
    public event Action OnClickMerchants;

    public override void Init(UIModel uiModel)
    {
        _sortie.onClick.AddListener(() => OnClickSortie?.Invoke());
        _storage.onClick.AddListener(() => OnClickStorage?.Invoke());
        _merchants.onClick.AddListener(() => OnClickMerchants?.Invoke());
        base.Init(uiModel);
    }

    public override void Terminate()
    {
        _sortie.onClick.RemoveAllListeners();
        _storage.onClick.RemoveAllListeners();
        _merchants.onClick.RemoveAllListeners();
        base.Terminate();
    }
}

public class MainMenuModel : UIModel
{
    
}