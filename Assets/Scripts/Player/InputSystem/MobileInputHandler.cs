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
        public void Init()
        {
            throw new NotImplementedException();
        }
    }
}