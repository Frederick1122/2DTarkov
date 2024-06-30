using System.Collections.Generic;
using Base.MVC;
using ConfigScripts;
using Managers.SaveLoadManagers;
using UI;
using UI.Base;
using UI.Inventory;
using UnityEngine;

public class LootBoxWindowController : WindowController //<LootBoxWindowView, LootWindowModel>
{
    [SerializeField] private ItemInformationPanelView _itemInformationPanelView;
    
    private ItemCellView _currentCellView;
    private List<ItemConfig> _lootItems = new List<ItemConfig>();
    private int _lootBoxIndex;

    private void Start()
    {
        GetView<LootBoxWindowView>().ClickOnCellAction += ClickOnCellAction;
        GetView<LootBoxWindowView>().OnTakeAction += Take;

        _itemInformationPanelView.UpdateView(new ItemInformationPanelModel());
    }

    private void OnDisable()
    {
        GetView<LootBoxWindowView>().ClickOnCellAction -= ClickOnCellAction;
        GetView<LootBoxWindowView>().OnTakeAction -= Take;
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
        
        UpdateView();
    }

    private void ClickOnCellAction(ItemCellView cellView)
    {
        var cellItem = cellView.GetItem(); 
        _currentCellView = _currentCellView == cellView ? null : cellView;
        
        if (_currentCellView != null) 
            _itemInformationPanelView.UpdateView(new ItemInformationPanelModel(cellItem.icon, cellItem.name, cellItem.description));

        GetView<LootBoxWindowView>().Refresh(_currentCellView != null);
    }

    private void Take()
    {
        InventorySaveLoadManager.Instance.AddItem(_currentCellView.GetItem(), _currentCellView.GetCount());
        GetView<LootBoxWindowView>().DeleteCell(_currentCellView);
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

    protected override UIModel GetViewData()
    {
        return new LootWindowModel(_lootItems);
    }
}
