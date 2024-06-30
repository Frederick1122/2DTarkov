
using System;
using System.Collections.Generic;
using Base.MVC;
using ConfigScripts;
using UI.Base;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxWindowView : WindowView //<LootWindowModel>
{
    public event Action<ItemCellView> ClickOnCellAction;
    public event Action OnTakeAction;
    
    [SerializeField] private ItemCellView _cellViewPrefab;
    [SerializeField] private GridLayoutGroup _itemLayoutGroup;
    [SerializeField] private Button _takeButton;

    private List<ItemCellView> _cells = new List<ItemCellView>();

    private void Start() => _takeButton.onClick.AddListener(() => OnTakeAction?.Invoke());
    
    public override void UpdateView(UIModel uiModel)
    {
        var castData = (LootWindowModel) uiModel;
        
        for (int i = _cells.Count - 1; i >= 0; i--)
        {
            var cell = _cells[i];
            _cells.RemoveAt(i);
            Destroy(cell.gameObject);
        }
        
        _cells.Clear();
        Refresh(false);
        
        foreach (var item in castData.lootItems)
        {
            var newCell = Instantiate(_cellViewPrefab, _itemLayoutGroup.transform);
            newCell.Init(new ItemCellModel(item, item.baseStack));
            newCell.GetButton().onClick.AddListener(() => ClickOnCellAction?.Invoke(newCell));
            _cells.Add(newCell);
        }
        
        base.UpdateView(uiModel);
    }

    public void Refresh(bool isItemSelected)
    {
        _takeButton.gameObject.SetActive(isItemSelected);
    }

    public void DeleteCell(ItemCellView cell)
    {
        _cells.Remove(cell);
        Destroy(cell.gameObject);
        Refresh(false);
    }
}

public class LootWindowModel : UIModel
{
    public List<ItemConfig> lootItems;

    public LootWindowModel(List<ItemConfig> lootItems)
    {
        this.lootItems = lootItems;
    }
}
