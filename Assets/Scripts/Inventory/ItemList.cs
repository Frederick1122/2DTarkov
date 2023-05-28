using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ItemList", menuName = "Item/ItemList")]
public class ItemList : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
    [SerializeField] private List<string> _paths = new List<string>();

    [ContextMenu("GetAllItems")]
    private void GetAllItems()
    {
        itemList.Clear();

        if (_paths.Count == 0)
        {
            itemList.AddRange(Resources.LoadAll<Item>(""));
            return;
        }

        foreach (var path in _paths) 
            itemList.AddRange(Resources.LoadAll<Item>(path));
    }
}