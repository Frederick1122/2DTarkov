using System;
using Base.MVC;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

public class WindowView : UIView
{
    //optional functionality
    
    public event Action OnClickExitWindow;
    
    [SerializeField] private Button _exitButton;


    public override void Init(UIModel uiModel)
    {
        _exitButton?.onClick.AddListener(() => OnClickExitWindow?.Invoke());
        base.Init(uiModel);
    }

    internal virtual void OnDestroy()
    {
        _exitButton?.onClick.RemoveAllListeners();
    }
}