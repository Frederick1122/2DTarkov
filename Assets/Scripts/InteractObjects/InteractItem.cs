using System;
using Base;
using UnityEngine;

namespace InteractObjects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class InteractItem : MonoBehaviour, IInteract
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count;
        [Space]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public void Init(Item item, int count, Vector3 position, Sprite sprite)
        {
            _item = item;
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
            InventorySaveLoadManager.Instance.AddItem(_item, _count);
            Destroy(gameObject);
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
