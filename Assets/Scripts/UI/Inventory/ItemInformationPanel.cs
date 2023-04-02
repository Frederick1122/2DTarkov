using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
   public class ItemInformationPanel : MonoBehaviour
   {
      [SerializeField] private Image _icon;
      [SerializeField] private TMP_Text _name;
      [SerializeField] private TMP_Text _description;

      public void SetNewInformation(Sprite icon = null, string name = "", string description = "")
      {
         if (icon == null)
         {
            var tempColor = _icon.color;
            tempColor.a = icon == null ? 0f : 1f;
            _icon.color = tempColor;
         }
         
         _icon.sprite = icon;
         _name.text = name;
         _description.text = description;
      }
   }
}
