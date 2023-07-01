using System;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowView : UIView
{
    public event Action OnInteractWithItem;
    public event Action OnDrop;

    [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    [SerializeField] private ActionButtonsView _actionButtonsView;

    private void OnEnable()
    {
        _actionButtonsView.OnUseAction += Interact;
        _actionButtonsView.OnEquipAction += Interact;
        _actionButtonsView.OnDropAction += Drop;
    }

    private void OnDisable()
    {
        _actionButtonsView.OnUseAction -= Interact;
        _actionButtonsView.OnEquipAction -= Interact;
        _actionButtonsView.OnDropAction -= Drop;
    }

    public void SetItemInformation(Sprite icon = null, string name = "", string description = "")
    {
        _itemInformationPanelView.SetNewInformation(icon, name, description);
    }

    public void SetActionButtonVisible(bool isActiveUseButton = false, bool isActiveEquipButton = false,
        bool isActiveDivideButton = false, bool isActiveDropButton = false)
    {
        _actionButtonsView.SetActiveButtons(isActiveUseButton, isActiveEquipButton, isActiveDivideButton, isActiveDropButton);
    }

    public GameObject GetInventoryLayout()
    {
        return _inventoryLayoutGroup.gameObject;
    }

    private void Interact()
    {
        OnInteractWithItem?.Invoke();
    }

    private void Drop()
    {
        OnDrop?.Invoke();
    }
}
