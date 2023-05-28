using System.Collections.Generic;
using Base;
using UI;
using UnityEngine;

namespace InteractObjects
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SaveInChunk))]
    public class InteractLootBox : MonoBehaviour, IInteract
    {
        [Range(0, 50)]
        [SerializeField] private int _minNumberOfItems;
        [Range(0, 50)]
        [SerializeField] private int _maxNumberOfItems;
        [SerializeField] private ItemList _itemList;

        [Header("Auto-serialised field")]
        [SerializeField] private SaveInChunk _saveInChunk;
        private List<Item> _items = new List<Item>();

        private void OnValidate() => UpdateFields();
        
        public void Interact()
        {
            GenerateNewItems();
            UIMainController.Instance.OpenLootBoxUI(_items);
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
        }
        
        private void UpdateFields()
        {
            if (_saveInChunk == null || _saveInChunk == default) 
                _saveInChunk = GetComponent<SaveInChunk>();

            if (_minNumberOfItems > _maxNumberOfItems)
                _maxNumberOfItems = _minNumberOfItems;
        }
    }
}