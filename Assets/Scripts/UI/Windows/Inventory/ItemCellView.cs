using System;
using Base.MVC;
using ConfigScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class ItemCellView : UIView
    {
        public event Action<ItemCellView> OnClickCell = delegate { };
        
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _countText;

        private ItemConfig _itemConfig;
        private Button _button;
        private int _count;

        public override void Init(UIModel uiModel)
        {
            if (_button == null) 
                _button = GetComponent<Button>();
            
            _button.onClick.AddListener(ClickOnCell);

            base.Init(uiModel);
        }

        public override void UpdateView(UIModel uiModel)
        {
            var castData = (ItemCellModel) uiModel;

            _itemConfig = castData.ItemConfig;
            _icon.sprite = castData.ItemConfig.icon;
            _count = castData.Count;
            _countText.text = castData.Count.ToString();
            
            base.UpdateView(uiModel);
        }

        public ItemConfig GetItem() => _itemConfig;

        public int GetCount() => _count;

        public void SetCount( int count ) => _count = count;

        private void ClickOnCell()
        {
            OnClickCell?.Invoke(this);
        }
    }
    
    public class ItemCellModel : UIModel
    {
        public ItemConfig ItemConfig;
        public int Count;

        public ItemCellModel()
        {
            
        }

        public ItemCellModel(ItemConfig itemConfig, int count)
        {
            ItemConfig = itemConfig;
            Count = count;
        }
    }
}
