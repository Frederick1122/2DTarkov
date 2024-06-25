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

        public InputType CurrentInputType { get; private set; }

        public event Action OnSwipeWeaponAction = delegate {  };
        public event Action OnOpenInventoryAction = delegate {  };
        public event Action OnInteractAction = delegate {  };
        public event Action OnOpenIngameMenuAction = delegate {  };
        public event Action OnReloadWeaponAction = delegate {  };
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
            CurrentInputType = _inputHandler.InputType;
            _inputHandler.Init();

            _inputHandler.OnVerticalMoveChange += OnVerticalMoveChange;
            _inputHandler.OnHorizontalMoveChange += OnHorizontalMoveChange;
            _inputHandler.OnHorizontalRotateChange += OnHorizontalRotateChange;
            _inputHandler.OnAttackChange += ShootChange;

            _inputHandler.OnOpenInventoryAction += OpenInventory;
            _inputHandler.OnOpenIngameMenuAction += OpenIngameMenu;
            _inputHandler.OnReloadWeaponAction += ReloadWeapon;
            _inputHandler.OnInteractAction += Interact;
            _inputHandler.OnSwipeWeaponAction += SwipeWeapon;
        }
        
        protected virtual void OnDestroy()
        {
            if (_inputHandler == null)
                return;

            _inputHandler.OnVerticalMoveChange -= OnVerticalMoveChange;
            _inputHandler.OnHorizontalMoveChange -= OnHorizontalMoveChange;
            _inputHandler.OnHorizontalRotateChange -= OnHorizontalRotateChange;
            _inputHandler.OnAttackChange -= ShootChange;
            
            _inputHandler.OnOpenInventoryAction -= OpenInventory;
            _inputHandler.OnOpenIngameMenuAction -= OpenIngameMenu;
            _inputHandler.OnReloadWeaponAction -= ReloadWeapon;
            _inputHandler.OnInteractAction -= Interact;
            _inputHandler.OnSwipeWeaponAction -= SwipeWeapon;
        }

        private void OnVerticalMoveChange(float value) =>
            _verticalMoveInput = value;

        private void OnHorizontalMoveChange(float value) =>
            _horizontalMoveInput = value;
        
        private void OnHorizontalRotateChange(float value) =>
            _horizontalRotateInput = value;

        private void ShootChange(bool value) =>
            _shootInput = value;

        private void OpenInventory() => 
            OnOpenInventoryAction?.Invoke();
        
        private void OpenIngameMenu() => 
            OnOpenIngameMenuAction?.Invoke();
        
        private void ReloadWeapon() => 
            OnReloadWeaponAction?.Invoke();

        private void Interact() =>
            OnInteractAction?.Invoke();

        private void SwipeWeapon() =>
            OnSwipeWeaponAction?.Invoke();
    }
}