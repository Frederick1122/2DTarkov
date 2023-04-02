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
        
        public void Init(ItemInformationPanel itemInformationPanel,Item item, int count = 1)
        {
            _item = item;
            _icon.sprite = item.icon;
            _count.text = count.ToString();

            if (_button == null) 
                _button = GetComponent<Button>();
            
            _button.onClick.AddListener(() => itemInformationPanel.SetNewInformation(item.icon, item.name, item.description));
        }

        public Image GetImage() => _icon;

        public TMP_Text GetCount() => _count;
    }
}
