using System.Collections.Generic;
using Base;
using UI;
using UI.Inventory;
using UnityEngine;

public class LootBoxWindowController : WindowController<LootBoxWindowView>
{
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    
    private ItemCellView _currentCellView;
    private List<Item> _lootItems = new List<Item>();
    private int _lootBoxIndex;

    private void Start()
    {
        _view.ClickOnCellAction += ClickOnCellAction;
        _view.OnTakeAction += Take;

        _itemInformationPanelView.SetNewInformation();
    }

    private void OnDisable()
    {
        _view.ClickOnCellAction -= ClickOnCellAction;
        _view.OnTakeAction -= Take;
    }

    public void Init(int lootBoxIndex, List<Item> lootItems)
    {
        if (lootItems.Count == 0)
        {
            _itemInformationPanelView.SetNewInformation(null,"","Box is empty");
            return;
        }
        _lootItems = lootItems;
        _lootBoxIndex = lootBoxIndex;
        
        _view.Init(lootItems);
    }

    private void ClickOnCellAction(ItemCellView cellView)
    {
        var cellItem = cellView.GetItem(); 
        _currentCellView = _currentCellView == cellView ? null : cellView;
        
        if (_currentCellView != null) 
            _itemInformationPanelView.SetNewInformation(cellItem.icon, cellItem.name, cellItem.description);

        _view.Refresh(_currentCellView != null);
    }

    private void Take()
    {
        Inventory.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount());
        _view.DeleteCell(_currentCellView);
        _itemInformationPanelView.SetNewInformation();
        
        _lootItems.Remove(_currentCellView.GetItem());
        Chunks.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
        
        if (_lootItems.Count == 0) 
            _itemInformationPanelView.SetNewInformation(null, "", "Box is empty");
        else
            _itemInformationPanelView.SetNewInformation();

    }

    public override void Show()
    {
        base.Show();
        
        _itemInformationPanelView.SetNewInformation();
    }

    public override void Hide()
    {
        if (_lootBoxIndex != 0)
        {
            Chunks.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
            _lootBoxIndex = 0;
        }
        
        base.Hide();
    }
}
