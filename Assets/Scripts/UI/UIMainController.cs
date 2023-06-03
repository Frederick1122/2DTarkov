using System.Collections.Generic;
using Base;
using UI.Base;
using UI.Inventory;
using UnityEngine;

namespace UI
{
  public class UIMainController : Singleton<UIMainController>
  {
    [SerializeField] private BaseUIWindowController _baseUIWindowController;
    [SerializeField] private InventoryWindowController _inventoryWindowController;
    [SerializeField] private LootBoxWindowController _lootBoxWindowController;

    private void Start() => OpenBaseUI();

    public void OpenBaseUI()
    {
      CloseAllUI();
    
      GameManager.Instance.GetPlayer().isFreeze = false;
      _baseUIWindowController.OpenWindow();
    }

    public void OpenInventoryUI()
    {
      CloseAllUI();
      
      GameManager.Instance.GetPlayer().isFreeze = true;
      
      _inventoryWindowController.OpenWindow();
    }

    public void OpenLootBoxUI(int lootBoxIndex, List<Item> lootItems)
    {
      CloseAllUI();
      
      GameManager.Instance.GetPlayer().isFreeze = true;
      
      _lootBoxWindowController.OpenWindow();
      _lootBoxWindowController.Init(lootBoxIndex, lootItems);
    }

    public BaseUIWindowController GetBaseUI() => _baseUIWindowController;
    
    private void CloseAllUI()
    {
      _baseUIWindowController.CloseWindow();
      _inventoryWindowController.CloseWindow();
      _lootBoxWindowController.CloseWindow();
    }
  }
}
