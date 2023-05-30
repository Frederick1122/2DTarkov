using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class ItemCellView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private Item _item;
        private Button _button;
        private int _count;
        
        public void Init(Item item, int count = 1)
        {
            _item = item;
            _icon.sprite = item.icon;
            _count = count;
            _countText.text = count.ToString();

            if (_button == null) 
                _button = GetComponent<Button>();
        }

        public Button GetButton() => _button;

        public Item GetItem() => _item;

        public int GetCount() => _count;

        public void SetCount( int count ) => _count = count;
    }
}
