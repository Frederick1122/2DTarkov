using System;
using System.Collections.Generic;
using ConfigScripts;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ItemList", menuName = "Item/ItemList")]
public class ItemList : ScriptableObject
{
    public List<ItemConfig> itemList = new List<ItemConfig>();
    [SerializeField] private List<string> _paths = new List<string>();

    [ContextMenu("GetAllItems")]
    private void GetAllItems()
    {
        itemList.Clear();

        if (_paths.Count == 0)
        {
            itemList.AddRange(Resources.LoadAll<ItemConfig>(""));
            return;
        }

        foreach (var path in _paths) 
            itemList.AddRange(Resources.LoadAll<ItemConfig>(path));
    }
}