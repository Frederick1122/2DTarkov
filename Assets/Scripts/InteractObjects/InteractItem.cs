using System;
using Base;
using ConfigScripts;
using Managers.SaveLoadManagers;
using UnityEngine;

namespace InteractObjects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractItem : MonoBehaviour, IInteract
    {
        [SerializeField] private ItemConfig _itemConfig;
        [SerializeField] private int _count;
        [Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public void Init(ItemConfig itemConfig, int count, Vector3 position, Sprite sprite)
        {
            _itemConfig = itemConfig;
            _count = count;
            transform.position = position;
            
            if (sprite != null)
            {
                _spriteRenderer.sprite = sprite;
            }
            
            var boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
            boxCollider2D.isTrigger = true;
        }
        
        public void Interact()
        {
            InventorySaveLoadManager.Instance.AddItem(_itemConfig, _count);
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
