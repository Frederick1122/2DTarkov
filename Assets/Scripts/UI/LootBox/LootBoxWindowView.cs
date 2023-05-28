
using System;
using System.Collections.Generic;
using UI.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxWindowView : MonoBehaviour
{
    public event Action<ItemCellView> ClickOnCellAction;
    public event Action OnTakeAction;
    
    [SerializeField] private ItemCellView _cellViewPrefab;
    [SerializeField] private GridLayoutGroup _itemLayoutGroup;
    [SerializeField] private Button _takeButton;

    private List<ItemCellView> _cells = new List<ItemCellView>();

    private void Start() => _takeButton.onClick.AddListener(() => OnTakeAction?.Invoke());

    public void Init(List<Item> lootItems)
    {
        for (int i = _cells.Count - 1; i >= 0; i--)
        {
            var cell = _cells[i];
            _cells.RemoveAt(i);
            Destroy(cell.gameObject);
        }
        
        _cells.Clear();
        Refresh(false);
        
        foreach (var item in lootItems)
        {
            var newCell = Instantiate(_cellViewPrefab, _itemLayoutGroup.transform);
            newCell.Init(item, item.baseStack);
            newCell.GetButton().onClick.AddListener(() => ClickOnCellAction?.Invoke(newCell));
            _cells.Add(newCell);
        }
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
