using Base;
using UnityEngine;

namespace InteractObjects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractItem : MonoBehaviour, IInteract
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count;

        public void Init(Item item, int count)
        {
            _item = item;
            _count = count;
        }
        
        public void Interact()
        {
            InventoryManager.Instance.AddItem(_item, _count);
            Destroy(gameObject);
        }
    }
}
