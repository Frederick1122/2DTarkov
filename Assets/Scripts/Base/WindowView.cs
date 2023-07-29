using System;
using UnityEngine;
using UnityEngine.UI;

public class WindowView<T> : UIView<T> where T : UIModel
{
    //optional functionality
    
    public event Action OnClickExitWindow;
    
    [SerializeField] private Button _exitButton;

    internal virtual void Start()
    {
        _exitButton?.onClick.AddListener(() => OnClickExitWindow?.Invoke());
    }

    internal virtual void OnDestroy()
    {
        _exitButton?.onClick.RemoveAllListeners();
    }
}