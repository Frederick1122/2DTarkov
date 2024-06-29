using System.Collections.Generic;
using Base;
using ConfigScripts;
using Managers.SaveLoadManagers;
using Player.InputSystem;
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

        private IInputSystem _inputSystem;

        public void Init()
        {
            _inputSystem = GameBus.Instance.PlayerInputSystem;
        }

        private void Start() => OpenBaseUI();

        public void OpenBaseUI()
        {
            CloseAllUI();

            Cursor.lockState = CursorLockMode.Locked;

            GameBus.Instance.PlayerHumanoid.isFreeze = false;
            
            _inputSystem.OnOpenInventoryAction += OpenInventoryUI;
            _inputSystem.OnSwipeWeaponAction += EquipmentSaveLoadManager.Instance.SwipeWeapon;
            _inputSystem.OnReloadWeaponAction += EquipmentSaveLoadManager.Instance.ReloadWeapon;

            _baseUIWindowController.Show();
        }

        public void OpenInventoryUI()
        {
            CloseAllUI();

            Cursor.lockState = CursorLockMode.None;

            GameBus.Instance.PlayerHumanoid.isFreeze = true;

            _inventoryWindowController.Show();
        }

        public void OpenLootBoxUI(int lootBoxIndex, List<ItemConfig> lootItems)
        {
            CloseAllUI();

            Cursor.lockState = CursorLockMode.None;

            GameBus.Instance.PlayerHumanoid.isFreeze = true;

            _lootBoxWindowController.Init(lootBoxIndex, lootItems);
            _lootBoxWindowController.Show();
        }

        public void OpenEndGameUI(string endText)
        {
            Cursor.lockState = CursorLockMode.None;

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
            _inputSystem.OnOpenInventoryAction -= OpenInventoryUI;
            _inputSystem.OnSwipeWeaponAction -= EquipmentSaveLoadManager.Instance.SwipeWeapon;
            _inputSystem.OnReloadWeaponAction -= EquipmentSaveLoadManager.Instance.ReloadWeapon;
            
            _baseUIWindowController.Hide();
            _inventoryWindowController.Hide();
            _lootBoxWindowController.Hide();
            _endGameUIController.Hide();
        }
    }
}