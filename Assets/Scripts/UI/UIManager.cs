using UI.Inventory;
using UnityEngine;

public class UIManager : MonoBehaviour
{
  [SerializeField] private GameObject _baseUI;
  [SerializeField] private InventoryWindowController _inventoryWindowController;

  public void OpenBaseUI()
  {
    CloseAllUI();
    
    GameManager.Instance.GetPlayer().isFreeze = false;
    _baseUI.SetActive(true);
  }

  public void OpenInventoryUI()
  {
      CloseAllUI();
      
      GameManager.Instance.GetPlayer().isFreeze = true;
      
      _inventoryWindowController.gameObject.SetActive(true);
      _inventoryWindowController.Refresh();
  }

  private void CloseAllUI()
  {
    _baseUI.SetActive(false);
    _inventoryWindowController.gameObject.SetActive(false);
  }
}
