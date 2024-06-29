using ConfigScripts;
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

        private ItemConfig _itemConfig;
        private Button _button;
        private int _count;
        
        public void Init(ItemConfig itemConfig, int count = 1)
        {
            _itemConfig = itemConfig;
            _icon.sprite = itemConfig.icon;
            _count = count;
            _countText.text = count.ToString();

            if (_button == null) 
                _button = GetComponent<Button>();
        }

        public Button GetButton() => _button;

        public ItemConfig GetItem() => _itemConfig;

        public int GetCount() => _count;

        public void SetCount( int count ) => _count = count;
    }
}
