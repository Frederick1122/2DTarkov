using Base;
using UnityEngine;

namespace InteractObjects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractItem : ObjectWithInteraction
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count;
        
        public override void Interact()
        {
            InventoryManager.Instance.AddItem(_item, _count);
            Destroy(gameObject);
        }
    }
}
