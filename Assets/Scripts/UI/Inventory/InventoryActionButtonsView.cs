using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryActionButtonsView : MonoBehaviour
{
    public event Action onUseAction;
    public event Action onEquipAction;
    public event Action onDivideAction;
    public event Action onDropAction;
    
    [Space] [Header("Action Buttons")]
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _equipButton;
    [SerializeField] private Button _divideButton;
    [SerializeField] private Button _dropButton;

    private void Start()
    {
        _useButton.onClick.AddListener(() =>
        {
            onUseAction?.Invoke();
        });
        _equipButton.onClick.AddListener(() =>
        {
            onEquipAction?.Invoke();
        });
        _divideButton.onClick.AddListener(() =>
        {
            onDivideAction?.Invoke();
        });
        _dropButton.onClick.AddListener(() =>
        {
            onDropAction?.Invoke();
        });
    }

    public void SetActiveButtons(bool isActiveUseButton = false, bool isActiveEquipButton = false,
        bool isActiveDivideButton = false, bool isActiveDropButton = false)
    {
        _useButton.gameObject.SetActive(isActiveUseButton);
        _equipButton.gameObject.SetActive(isActiveEquipButton);
        _divideButton.gameObject.SetActive(isActiveDivideButton);
        _dropButton.gameObject.SetActive(isActiveDropButton);
    }
}
