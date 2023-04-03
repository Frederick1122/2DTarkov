using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class InventoryWindowCell : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        private Item _item;
        private Button _button;
        
        public void Init(Item item, int count = 1)
        {
            _item = item;
            _icon.sprite = item.icon;
            _count.text = count.ToString();

            if (_button == null) 
                _button = GetComponent<Button>();
        }

        public Button GetButton() => _button;

        public Item GetItem() => _item;
    }
}
