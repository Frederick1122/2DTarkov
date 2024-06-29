using System.Collections.Generic;
using ConfigScripts;
using Managers.SaveLoadManagers;
using UI;
using UI.Inventory;
using UnityEngine;

public class LootBoxWindowController : WindowController<LootBoxWindowView, LootWindowModel>
{
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    
    private ItemCellView _currentCellView;
    private List<ItemConfig> _lootItems = new List<ItemConfig>();
    private int _lootBoxIndex;

    private void Start()
    {
        _view.ClickOnCellAction += ClickOnCellAction;
        _view.OnTakeAction += Take;

        _itemInformationPanelView.UpdateView(new ItemInformationPanelModel());
    }

    private void OnDisable()
    {
        _view.ClickOnCellAction -= ClickOnCellAction;
        _view.OnTakeAction -= Take;
    }

    public void Init(int lootBoxIndex, List<ItemConfig> lootItems)
    {
        if (lootItems.Count == 0)
        {
            _itemInformationPanelView.UpdateView(new ItemInformationPanelModel(null,"","Box is empty"));
            return;
        }
        _lootItems = lootItems;
        _lootBoxIndex = lootBoxIndex;
        
        _view.UpdateView(new LootWindowModel(lootItems));
    }

    private void ClickOnCellAction(ItemCellView cellView)
    {
        var cellItem = cellView.GetItem(); 
        _currentCellView = _currentCellView == cellView ? null : cellView;
        
        if (_currentCellView != null) 
            _itemInformationPanelView.UpdateView(new ItemInformationPanelModel(cellItem.icon, cellItem.name, cellItem.description));

        _view.Refresh(_currentCellView != null);
    }

    private void Take()
    {
        InventorySaveLoadManager.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount());
        _view.DeleteCell(_currentCellView);
        _itemInformationPanelView.UpdateView(new ItemInformationPanelModel());
        
        _lootItems.Remove(_currentCellView.GetItem());
        ChunksSaveLoadManager.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
        
        if (_lootItems.Count == 0) 
            _itemInformationPanelView.UpdateView(new ItemInformationPanelModel(null, "", "Box is empty"));
        else
            _itemInformationPanelView.UpdateView(new ItemInformationPanelModel());
    }

    public override void Show()
    {
        base.Show();
        
        _itemInformationPanelView.UpdateView(new ItemInformationPanelModel());
    }

    public override void Hide()
    {
        if (_lootBoxIndex != 0)
        {
            ChunksSaveLoadManager.Instance.SaveLootBox(_lootBoxIndex, _lootItems);
            _lootBoxIndex = 0;
        }
        
        base.Hide();
    }
}
