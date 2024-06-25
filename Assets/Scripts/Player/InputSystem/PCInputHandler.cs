using System;
using UnityEngine;

namespace Player.InputSystem
{
    public class PCInputHandler : MonoBehaviour, IInputHandler
    {
        private const string VERTICAL_AXIS = "Vertical";
        private const string HORIZONTAl_AXIS = "Horizontal";
        private const string HORIZONTAl_AXIS_MOUSE = "Mouse X";
        private const string SCROLL_WHEEL_MOUSE = "Mouse ScrollWheel";

        private int LEFT_MOUSE_BUTTON_IDX = 0;
        private KeyCode OPEN_INVENTORY_BUTTON = KeyCode.I;
        private KeyCode OPEN_INGAME_MENU_BUTTON = KeyCode.Escape;
        private KeyCode RELOAD_WEAPON_BUTTON = KeyCode.R;
        private KeyCode INTERACT_BUTTON = KeyCode.E;

        public event Action OnSwipeWeaponAction;
        public event Action OnOpenInventoryAction;
        public event Action OnOpenIngameMenuAction;
        public event Action OnReloadWeaponAction;
        public event Action OnInteractAction;
        public event Action<float> OnVerticalMoveChange;
        public event Action<float> OnHorizontalMoveChange;
        public event Action<float> OnHorizontalRotateChange;
        public event Action<bool> OnAttackChange;
        
        public InputType InputType => InputType.PC;

        public bool IsActive { get; set; }
        
        private float _verticalMove = 0;
        private float _horizontalMove = 0;
        private float _horizontalRotate = 0;
        private bool _isAttackActive = false;

        public void Init()
        {
            IsActive = false;
        }

        private void Update()
        {
            if (!IsActive)
                return;
            
            MoveLogic();
            RotateLogic();
            AttackLogic();
            OtherInputsLogic();
        }

        private void MoveLogic()
        {
            var vertA = Input.GetAxis(VERTICAL_AXIS);
            var horA = Input.GetAxis(HORIZONTAl_AXIS);
            
            if (_verticalMove != vertA)
            {
                _verticalMove = vertA;
                OnVerticalMoveChange?.Invoke(_verticalMove);
            }

            if (_horizontalMove != horA)
            {
                _horizontalMove = horA;
                OnHorizontalMoveChange?.Invoke(_horizontalMove);
            }
        }

        private void RotateLogic()
        {
            var horA = Input.GetAxis(HORIZONTAl_AXIS_MOUSE);

            if (_horizontalRotate != horA)
            {
                _horizontalRotate = horA;
                OnHorizontalRotateChange?.Invoke(-_horizontalRotate);
            }
        }

        private void AttackLogic()
        {
            var leftMouseInput = Input.GetMouseButton(LEFT_MOUSE_BUTTON_IDX);
            if (leftMouseInput != _isAttackActive)
            {
                _isAttackActive = leftMouseInput;
                OnAttackChange?.Invoke(_isAttackActive);
            }
        }

        private void OtherInputsLogic()
        {
            if (Input.GetKeyDown(OPEN_INVENTORY_BUTTON))
            {
                OnOpenInventoryAction?.Invoke();
            }
            
            if (Input.GetKeyDown(OPEN_INGAME_MENU_BUTTON))
            {
                OnOpenIngameMenuAction?.Invoke();
            }
            
            if (Input.GetKeyDown(RELOAD_WEAPON_BUTTON))
            {
                OnReloadWeaponAction?.Invoke();
            }
            
            if (Input.GetKeyDown(INTERACT_BUTTON))
            {
                OnInteractAction?.Invoke();
            }

            if (Input.GetAxis(SCROLL_WHEEL_MOUSE) != 0)
            {
                OnSwipeWeaponAction?.Invoke();
            }
        }
    }
}