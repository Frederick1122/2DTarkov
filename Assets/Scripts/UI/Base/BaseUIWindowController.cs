using System;
using Base;
using UnityEngine;

namespace UI.Base
{
    public class BaseUIWindowController : WindowController
    {
        [SerializeField] private BaseUIWindowView _baseUIWindowView;

        private void Start()
        {
            _baseUIWindowView.Init();
        }

        public void InitInteractButton(IInteract interact)
        {
            _baseUIWindowView.InitInteractButton(interact);
        }

        public void SetActiveInteractButton(bool isActive)
        {
            _baseUIWindowView.SetActiveInteractButton(isActive);
        }
    }
}
