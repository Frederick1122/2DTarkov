using System.Collections.Generic;
using Base;
using UI.Base;
using UnityEngine;

namespace UI
{
  public class UIManager : Singleton<UIManager>
  {
    [SerializeField] private BaseUIWindowController _baseUIWindowController;
    [SerializeField] private InventoryWindowController _inventoryWindowController;
    [SerializeField] private LootBoxWindowController _lootBoxWindowController;
    [SerializeField] private EndGameUIController _endGameUIController;
    
    public void Init()
    {
      //
    }
    
    private void Start() => OpenBaseUI();

    public void OpenBaseUI()
    {
      CloseAllUI();
    
      GameBus.Instance.PlayerHumanoid.isFreeze = false;
      _baseUIWindowController.Show();
    }

    public void OpenInventoryUI()
    {
      CloseAllUI();
      
      GameBus.Instance.PlayerHumanoid.isFreeze = true;
      
      _inventoryWindowController.Show();
    }

    public void OpenLootBoxUI(int lootBoxIndex, List<Item> lootItems)
    {
      CloseAllUI();
      
      GameBus.Instance.PlayerHumanoid.isFreeze = true;
      
      _lootBoxWindowController.Init(lootBoxIndex, lootItems);
      _lootBoxWindowController.Show();
    }

    public void OpenEndGameUI(string endText)
    {
        CloseAllUI();
        _endGameUIController.Init(endText);
        _endGameUIController.Show();
    }

    public BaseUIWindowController GetBaseUI()
    {
      return _baseUIWindowController;
    }

    private void CloseAllUI()
    {
      _baseUIWindowController.Hide();
      _inventoryWindowController.Hide();
      _lootBoxWindowController.Hide();
      _endGameUIController.Hide();
    }
  }
}
