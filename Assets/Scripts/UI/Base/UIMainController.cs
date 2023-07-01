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
    
      GameBus.Instance.GetPlayer().isFreeze = false;
      _baseUIWindowController.Show();
    }

    public void OpenInventoryUI()
    {
      CloseAllUI();
      
      GameBus.Instance.GetPlayer().isFreeze = true;
      
      _inventoryWindowController.Show();
    }

    public void OpenLootBoxUI(int lootBoxIndex, List<Item> lootItems)
    {
      CloseAllUI();
      
      GameBus.Instance.GetPlayer().isFreeze = true;
      
      _lootBoxWindowController.Show();
      _lootBoxWindowController.Init(lootBoxIndex, lootItems);
    }

    public BaseUIWindowController GetBaseUI() => _baseUIWindowController;
    
    private void CloseAllUI()
    {
      _baseUIWindowController.Hide();
      _inventoryWindowController.Hide();
      _lootBoxWindowController.Hide();
    }
  }
}
