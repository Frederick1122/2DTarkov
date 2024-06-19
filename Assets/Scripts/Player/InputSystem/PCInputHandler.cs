using System;
using UnityEngine;

namespace Player.InputSystem
{
    public class PCInputHandler : MonoBehaviour, IInputHandler
    {
        private const string VERTICAL_AXIS = "Vertical";
        private const string HORIZONTAl_AXIS = "Horizontal";
        private const string HORIZONTAl_AXIS_MOUSE = "Mouse X";

        private KeyCode OPEN_INVENTORY_BUTTON = KeyCode.I;
        private KeyCode OPEN_INGAME_MENU_BUTTON = KeyCode.Escape;
        private KeyCode RELOAD_WEAPON_BUTTON = KeyCode.R;
        
        public event Action OnOpenInventoryAction;
        public event Action OnOpenIngameMenuAction;
        public event Action OnReloadWeaponAction;
        public event Action<float> OnVerticalMoveChange;
        public event Action<float> OnHorizontalMoveChange;
        public event Action<float> OnHorizontalRotateChange;
        public event Action<bool> OnShootChange;
        
        public bool IsActive { get; set; }
        
        private float _verticalMove = 0;
        private float _horizontalMove = 0;
        private float _horizontalRotate = 0;

        private float _lastMousePositionX;
        
        public void Init()
        {
            IsActive = false;
            _verticalMove = 0;
            _horizontalMove = 0;
            _horizontalRotate = 0;
            _lastMousePositionX = Input.mousePosition.x;

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (!IsActive)
                return;
            
            MoveLogic();
            RotateLogic();
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
            
            // var currentMousePositionX = Input.mousePosition.x; 
            // var horizontalRotate = currentMousePositionX - _lastMousePositionX;
            //
            // if (_horizontalRotate != horizontalRotate)
            // {
            //     _horizontalRotate = horizontalRotate;
            //     OnHorizontalRotateChange?.Invoke(Math.Clamp(_horizontalRotate, -1, 1) );
            // }
            //
            // _lastMousePositionX = currentMousePositionX;
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
        }
    }
}