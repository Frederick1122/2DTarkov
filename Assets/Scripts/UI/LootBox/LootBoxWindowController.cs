using System.Collections.Generic;
using Base;
using UI;
using UI.Inventory;
using UnityEngine;

public class LootBoxWindowController : WindowController
{
    [SerializeField] private LootBoxWindowView _lootBoxWindowView;
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    
    private ItemCellView _currentCellView;
    private List<Item> _lootItems = new List<Item>();
    private int _lootBoxIndex;

    private void Start()
    {
        _lootBoxWindowView.ClickOnCellAction += ClickOnCellAction;
        _lootBoxWindowView.OnTakeAction += Take;

        _itemInformationPanelView.SetNewInformation();
    }

    private void OnDisable()
    {
        _lootBoxWindowView.ClickOnCellAction -= ClickOnCellAction;
        _lootBoxWindowView.OnTakeAction -= Take;
    }

    public void Init(int lootBoxIndex, List<Item> lootItems)
    {
        _lootItems = lootItems;
        _lootBoxIndex = lootBoxIndex;
        
        _lootBoxWindowView.Init(lootItems);
    }

    private void ClickOnCellAction(ItemCellView cellView)
    {
        var cellItem = cellView.GetItem(); 
        _currentCellView = _currentCellView == cellView ? null : cellView;
        
        if (_currentCellView != null) 
            _itemInformationPanelView.SetNewInformation(cellItem.icon, cellItem.name, cellItem.description);

        _lootBoxWindowView.Refresh(_currentCellView != null);
    }

    private void Take()
    {
        InventoryManager.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount());
        _lootBoxWindowView.DeleteCell(_currentCellView);
        _itemInformationPanelView.SetNewInformation();
        
        _lootItems.Remove(_currentCellView.GetItem());
        ChunkManager.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
    }

    public override void CloseWindow()
    {
        if (_lootBoxIndex != 0)
        {
            ChunkManager.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
            _lootBoxIndex = 0;
        }
        
        base.CloseWindow();
    }
}
