using System.Collections;
using System.Collections.Generic;
using UI.Inventory;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  [SerializeField] private GameObject _baseUI;
  [SerializeField] private InventoryWindow _inventoryWindow;

  public void OpenBaseUI()
  {
    CloseAllUI();
    
    GameManager.Instance.GetPlayer().IsFreezing(false);
    _baseUI.SetActive(true);
  }

  public void OpenInventoryUI()
  {
      CloseAllUI();
      
      GameManager.Instance.GetPlayer().IsFreezing(true);
      
      _inventoryWindow.Refresh();
      _inventoryWindow.gameObject.SetActive(true);
  }

  private void CloseAllUI()
  {
    _baseUI.SetActive(false);
    _inventoryWindow.gameObject.SetActive(false);
  }
}
