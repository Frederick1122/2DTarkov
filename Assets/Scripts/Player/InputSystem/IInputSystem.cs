﻿using System;

namespace Player.InputSystem
{
    public interface IInputSystem
    {
        public event Action OnSwipeWeaponAction;
        public event Action OnOpenInventoryAction;
        public event Action OnInteractAction;
        public event Action OnOpenIngameMenuAction;
        
        public event Action OnReloadWeaponAction;
        
        public bool IsActive { get; set; }

        public InputType CurrentInputType { get; }
        
        public void Init() { }

        public float VerticalMoveInput { get; }
        public float HorizontalMoveInput { get; }

        public float HorizontalRotateInput { get; }

        public bool ShootInput { get; }
    }
}