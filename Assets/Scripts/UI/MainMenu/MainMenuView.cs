using System;
using Base.MVC;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : UIView //<MainMenuModel>
{
    [SerializeField] private Button _sortie;
    [SerializeField] private Button _storage;
    [SerializeField] private Button _merchants;

    public event Action OnClickSortie;
    public event Action OnClickStorage;
    public event Action OnClickMerchants;

    private void OnEnable()
    {
        _sortie.onClick.AddListener(() => OnClickSortie?.Invoke());
        _storage.onClick.AddListener(() => OnClickStorage?.Invoke());
        _merchants.onClick.AddListener(() => OnClickMerchants?.Invoke());
    }

    private void OnDisable()
    {
        _sortie.onClick.RemoveAllListeners();
        _storage.onClick.RemoveAllListeners();
        _merchants.onClick.RemoveAllListeners();
    }
}

public class MainMenuModel : UIModel
{
    
}