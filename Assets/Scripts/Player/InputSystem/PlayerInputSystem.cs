using System;
using UnityEngine;

namespace Player.InputSystem
{
    public class PlayerInputSystem : MonoBehaviour, IInputSystem
    {
        public bool IsActive
        {
            get { return _inputHandler.IsActive; }
            set { _inputHandler.IsActive = value; }
        }
        
        public event Action OnOpenInventoryAction;
        public event Action OnOpenIngameMenuAction;
        public event Action OnReloadWeaponAction;
        public float VerticalMoveInput => _verticalMoveInput;
        public float HorizontalMoveInput => _horizontalMoveInput;
        public float HorizontalRotateInput => _horizontalRotateInput;
        public bool ShootInput => _shootInput;
        
        protected float _verticalMoveInput = 0;
        protected float _horizontalMoveInput = 0;
        protected float _horizontalRotateInput = 0;
        protected bool _shootInput = false;
        
        protected IInputHandler _inputHandler;

        private void Awake()
        {
#if UNITY_ANDROID 
            _inputHandler = gameObject.AddComponent(typeof(MobileInputHandler)) as IInputHandler;
#else
            _inputHandler = gameObject.AddComponent(typeof(PCInputHandler)) as IInputHandler;
#endif
            _inputHandler.Init();

            _inputHandler.OnVerticalMoveChange += OnVerticalMoveChange;
            _inputHandler.OnHorizontalMoveChange += OnHorizontalMoveChange;
            _inputHandler.OnHorizontalRotateChange += OnHorizontalRotateChange;
            _inputHandler.OnShootChange += OnShootChange;

            _inputHandler.OnOpenInventoryAction += OnOpenInventory;
            _inputHandler.OnOpenIngameMenuAction += OnOpenIngameMenu;
            _inputHandler.OnReloadWeaponAction += OnReloadWeapon;
        }
        
        protected virtual void OnDestroy()
        {
            if (_inputHandler == null)
                return;

            _inputHandler.OnVerticalMoveChange -= OnVerticalMoveChange;
            _inputHandler.OnHorizontalMoveChange -= OnHorizontalMoveChange;
            _inputHandler.OnHorizontalRotateChange -= OnHorizontalRotateChange;
            _inputHandler.OnShootChange -= OnShootChange;
            
            _inputHandler.OnOpenInventoryAction -= OnOpenInventory;
            _inputHandler.OnOpenIngameMenuAction -= OnOpenIngameMenu;
            _inputHandler.OnReloadWeaponAction -= OnReloadWeapon;
        }

        private void OnVerticalMoveChange(float value) =>
            _verticalMoveInput = value;

        private void OnHorizontalMoveChange(float value) =>
            _horizontalMoveInput = value;
        
        private void OnHorizontalRotateChange(float value) =>
            _horizontalRotateInput = value;

        private void OnShootChange(bool value) =>
            _shootInput = value;

        private void OnOpenInventory() => 
            OnOpenInventoryAction?.Invoke();
        
        private void OnOpenIngameMenu() => 
            OnOpenInventoryAction?.Invoke();
        
        private void OnReloadWeapon() => 
            OnOpenInventoryAction?.Invoke();
    }
}