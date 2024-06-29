using System;
using System.Collections.Generic;
using Base;
using ConfigScripts;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SaveInChunkLootBox))]
public class InteractLootBox : MonoBehaviour, IInteract
{
    [Range(0, 50)] [SerializeField] private int _minNumberOfItems;
    [Range(0, 50)] [SerializeField] private int _maxNumberOfItems;
    [SerializeField] private ItemList _itemList;

    [Header("Auto-serialised field")] [SerializeField]
    private SaveInChunkLootBox _saveInChunkLootBox;

    [SerializeField] private List<ItemConfig> _items = new List<ItemConfig>();
    private bool _isOpen = false;
    private int _index;
    
    private void OnValidate() => UpdateFields();

    private void Start() => _index = _saveInChunkLootBox.GetIndex();

    public void Init(List<ItemConfig> items = null)
    {
        if (items == null)
            return;

        _isOpen = true;
        _items = items;
    }

    public void Interact()
    {
        if (!_isOpen)
            GenerateNewItems();

        UIManager.Instance.OpenLootBoxUI(_index, _items);
    }

    private void GenerateNewItems()
    {
        _items.Clear();
        var count = Random.Range(_minNumberOfItems, _maxNumberOfItems);
        for (int i = 0; i < count; i++)
        {
            var newItem = _itemList.itemList[Random.Range(0, _itemList.itemList.Count)];
            _items.Add(newItem);
        }

        _isOpen = true;
    }

    private void UpdateFields()
    {
        if (_saveInChunkLootBox == null || _saveInChunkLootBox == default)
            _saveInChunkLootBox = GetComponent<SaveInChunkLootBox>();

        if (_minNumberOfItems > _maxNumberOfItems)
            _maxNumberOfItems = _minNumberOfItems;
    }
}