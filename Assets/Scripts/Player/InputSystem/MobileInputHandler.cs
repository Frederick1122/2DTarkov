using System;
using UnityEngine;

namespace Player.InputSystem
{
    public class MobileInputHandler : MonoBehaviour, IInputHandler
    {
        public event Action OnOpenInventoryAction;
        public event Action OnOpenIngameMenuAction;
        public event Action OnReloadWeaponAction;
        public event Action<float> OnVerticalMoveChange;
        public event Action<float> OnHorizontalMoveChange;
        public event Action<float> OnHorizontalRotateChange;
        public event Action<bool> OnShootChange;
        public bool IsActive { get; set; }

        private readonly float _deadZone = 0.1f;
        
        private bool _isMobile;

        private bool _isSwiping;
        private float _nonRotationZone;
        
        private float _verticalMove = 0;
        private float _horizontalMove = 0;
        private float _horizontalRotate = 0;

        private Vector2 _tapPosition;
        private int _touchNumber;
        private Vector2 _resolution;
        
        private Joystick _joystick;
        
        public void Init()
        {
            _joystick = GameBus.Instance.GetJoystick();
            
            _resolution = new Vector2(Screen.width, Screen.height);
            _nonRotationZone = _resolution.x / 4;
        }

        private void Update()
        {
            MoveLogic();
            RotateLogic();
        }
        
        private void MoveLogic()
        {
            var vertA = _joystick.Vertical;
            var horA = _joystick.Horizontal;
            
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
            if (!_isMobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _isSwiping = true;
                    _tapPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                    ResetSwipe();
            }
            else
            {
                for (var i = 0; i < Input.touchCount; i++)
                {
                    var touch = Input.GetTouch(i);
                    if (touch.position.x < _nonRotationZone)
                        continue;

                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            _isSwiping = true;
                            _tapPosition = touch.position;
                            _touchNumber = i;
                            break;
                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            ResetSwipe();
                            break;
                    }
                }
            }

            var swipeDelta = Vector2.zero;

            if (_tapPosition.x < _nonRotationZone)
            {
                OnHorizontalRotateChange?.Invoke(0);
                return;
            }
            
            if (_isSwiping)
            {
                var newTapPosition = new Vector2();

                if (!_isMobile && Input.GetMouseButton(0))
                {
                    newTapPosition = Input.mousePosition;
                    swipeDelta = newTapPosition - _tapPosition;
                }
                else if (Input.touchCount > _touchNumber)
                {
                    newTapPosition = Input.GetTouch(_touchNumber).position;

                    if(newTapPosition.x < _nonRotationZone)
                        return;

                    swipeDelta = newTapPosition - _tapPosition;
                }

                OnHorizontalRotateChange?.Invoke(-swipeDelta.x);
                _tapPosition = newTapPosition;
            }
        }

        private void ResetSwipe()
        {
            _isSwiping = false;
            _tapPosition = Vector2.zero;
            OnHorizontalRotateChange?.Invoke(0);
        }
    }
}