using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  [SerializeField] private GameObject _baseUI;
  [SerializeField] private GameObject _inventoryUI;

  public void OpenBaseUI()
  {
    CloseAllUI();
    
    GameManager.GetInstance().GetPlayer().IsFreezing(false);
    _baseUI.SetActive(true);
  }

  public void OpenInventoryUI()
  {
      CloseAllUI();
      
      GameManager.GetInstance().GetPlayer().IsFreezing(true);
      _inventoryUI.SetActive(true);
  }

  private void CloseAllUI()
  {
    _baseUI.SetActive(false);
    _inventoryUI.SetActive(false);
  }
}
