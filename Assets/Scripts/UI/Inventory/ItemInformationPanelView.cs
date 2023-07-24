using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
   public class ItemInformationPanelView : UIView<ItemInformationPanelModel>
   {
      [SerializeField] private Image _icon;
      [SerializeField] private TMP_Text _name;
      [SerializeField] private TMP_Text _description;

      public override void UpdateView(ItemInformationPanelModel uiModel)
      {
         var tempColor = _icon.color;
         tempColor.a = uiModel.icon == null ? 0f : 1f;
         _icon.color = tempColor;
         
         _icon.sprite = uiModel.icon;
         _name.text = uiModel.name;
         _description.text = uiModel.description;
         base.UpdateView(uiModel);
      }
   }

   public class ItemInformationPanelModel : UIModel
   {
      public Sprite icon;
      public string name;
      public string description;

      public ItemInformationPanelModel(Sprite icon = null, string name = "", string description = "")
      {
         this.icon = icon;
         this.name = name;
         this.description = description;
      }
   }
}
