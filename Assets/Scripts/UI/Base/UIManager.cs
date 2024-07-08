using System;
using System.Collections.Generic;
using Base;
using UI.Base;
using UI.Storage;
using UI.Windows.Inventory;
using UnityEngine;

namespace UI
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private BaseUIWindowController _baseUIWindowController;
        [SerializeField] private InventoryWindowController _inventoryWindowController;
        [SerializeField] private LootBoxWindowController _lootBoxWindowController;
        [SerializeField] private EndGameWindowController _endGameWindowController;
        [SerializeField] private MainMenuWindowController _mainMenuWindowController;
        [SerializeField] private StorageWindowController _storageWindowController;

        private Dictionary<Type, WindowController> _allControllers = new();
        private Stack<WindowController> _activeControllers = new();

        public void Init()
        {
            
        }

        //private void Start() => OpenBaseUI();

        // public void OpenBaseUI()
        // {
        //     CloseAllUI();
        //
        //     Cursor.lockState = CursorLockMode.Locked;
        //
        //     GameBus.Instance.PlayerHumanoid.isFreeze = false;
        //     
        //     _inputSystem.OnOpenInventoryAction += OpenInventoryUI;
        //     _inputSystem.OnSwipeWeaponAction += EquipmentSaveLoadManager.Instance.SwipeWeapon;
        //     _inputSystem.OnReloadWeaponAction += EquipmentSaveLoadManager.Instance.ReloadWeapon;
        //
        //     _baseUIWindowController.Show();
        // }
        //
        // public void OpenInventoryUI()
        // {
        //     CloseAllUI();
        //
        //     Cursor.lockState = CursorLockMode.None;
        //
        //     GameBus.Instance.PlayerHumanoid.isFreeze = true;
        //
        //     _inventoryWindowController.Show();
        // }
        //
        // public void OpenLootBoxUI(int lootBoxIndex, List<ItemConfig> lootItems)
        // {
        //     CloseAllUI();
        //
        //     Cursor.lockState = CursorLockMode.None;
        //
        //     GameBus.Instance.PlayerHumanoid.isFreeze = true;
        //
        //     _lootBoxWindowController.Init(lootBoxIndex, lootItems);
        //     _lootBoxWindowController.Show();
        // }
        //
        // public void OpenEndGameUI(string endText)
        // {
        //     Cursor.lockState = CursorLockMode.None;
        //
        //     CloseAllUI();
        //     endGameWindowController.Init(endText);
        //     endGameWindowController.Show();
        // }

        // public BaseUIWindowController GetBaseUI()
        // {
        //     return _baseUIWindowController;
        // }

        public T GetWindow<T>() where T : WindowController
        {
            if (_allControllers.ContainsKey(typeof(T)))
            {
                Debug.LogAssertion($"WindowController with type {typeof(T)} not found in UIManager");
                return null;
            }

            return (T)_allControllers[typeof(T)];
        }
        
        public void OpenWindow<T>() where T : WindowController
        {
            if (_allControllers.ContainsKey(typeof(T)))
            {
                Debug.LogAssertion($"WindowController with type {typeof(T)} not found in UIManager");
                return;
            }
            
            CloseLastUI();
            var newActiveController = _allControllers[typeof(T)];
            newActiveController.Show();
            _activeControllers.Push(newActiveController);
        }

        public void CloseWindow()
        {
            var lastWindow = _activeControllers.Pop();
            lastWindow.Hide();
        }
        
        private void CloseLastUI()
        {
            // _inputSystem.OnOpenInventoryAction -= OpenInventoryUI;
            // _inputSystem.OnSwipeWeaponAction -= EquipmentSaveLoadManager.Instance.SwipeWeapon;
            // _inputSystem.OnReloadWeaponAction -= EquipmentSaveLoadManager.Instance.ReloadWeapon;
            //
            // _baseUIWindowController.Hide();
            // _inventoryWindowController.Hide();
            // _lootBoxWindowController.Hide();
            // endGameWindowController.Hide();
            
            _activeControllers.Peek().Hide();
        }
    }
}