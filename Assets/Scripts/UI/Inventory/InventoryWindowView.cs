using System;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowView : WindowView<InventoryWindowModel>
{
    public event Action OnInteractWithItem;
    public event Action OnDrop;
    public event Action OnMove;

    [SerializeField] private GridLayoutGroup _inventoryLayoutGroup;
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    [SerializeField] private ActionButtonsView _actionButtonsView;

    internal void OnEnable()
    {
        _actionButtonsView.OnUseAction += Interact;
        _actionButtonsView.OnEquipAction += Interact;
        _actionButtonsView.OnDropAction += Drop;
        _actionButtonsView.OnMoveAction += Move;
    }

    internal void OnDisable()
    {
        _actionButtonsView.OnUseAction -= Interact;
        _actionButtonsView.OnEquipAction -= Interact;
        _actionButtonsView.OnDropAction -= Drop;
        _actionButtonsView.OnMoveAction -= Move;
    }

    public override void UpdateView(InventoryWindowModel uiModel)
    {
        base.UpdateView(uiModel);
        _itemInformationPanelView.UpdateView(uiModel.itemInformationPanelModel);
        _actionButtonsView.SetActiveButtons(uiModel.isActiveUseButton, uiModel.isActiveEquipButton,
            uiModel.isActiveDivideButton, uiModel.isActiveDropButton, uiModel.isActiveMoveToStorageButton);
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

    private void Move()
    {
        OnMove?.Invoke();
    }
}

public class InventoryWindowModel : UIModel
{
    public ItemInformationPanelModel itemInformationPanelModel;
    public bool isActiveUseButton;
    public bool isActiveEquipButton;
    public bool isActiveDivideButton;
    public bool isActiveDropButton;
    public bool isActiveMoveToStorageButton;
    
    public InventoryWindowModel()
    {
        this.itemInformationPanelModel = new ItemInformationPanelModel();
        this.isActiveUseButton = false;
        this.isActiveEquipButton = false;
        this.isActiveDivideButton = false;
        this.isActiveDropButton = false;
        this.isActiveMoveToStorageButton = false;
    }
    
    public InventoryWindowModel(ItemInformationPanelModel itemInformationPanelModel = null,
        bool isActiveUseButton = false, bool isActiveEquipButton = false, bool isActiveDivideButton = false,
        bool isActiveDropButton = false, bool isActiveMoveToStorageButton = false)
    {
        this.itemInformationPanelModel = itemInformationPanelModel ?? new ItemInformationPanelModel();
        this.isActiveUseButton = isActiveUseButton;
        this.isActiveEquipButton = isActiveEquipButton;
        this.isActiveDivideButton = isActiveDivideButton;
        this.isActiveDropButton = isActiveDropButton;
        this.isActiveMoveToStorageButton = isActiveMoveToStorageButton;
    }

    public InventoryWindowModel(Sprite icon = null, string name = "", string description = "")
    {
        itemInformationPanelModel = new ItemInformationPanelModel(icon, name, description);
    }
}