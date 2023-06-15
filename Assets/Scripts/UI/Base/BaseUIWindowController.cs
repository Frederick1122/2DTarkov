using Base;
using UnityEngine;

namespace UI.Base
{
    public class BaseUIWindowController : WindowController
    {
        [SerializeField] private BaseUIWindowView _baseUIWindowView;
        [SerializeField] private HealthBarView _healthBarView;
        private void Start()
        {
            _baseUIWindowView.Init();
            Player.Instance.OnHpChanged += SetHp;
            SetHp(Player.Instance.GetHp());
        }

        private void OnDisable()
        {
            Player.Instance.OnHpChanged -= SetHp;
        }

        public void InitInteractButton(IInteract interact)
        {
            _baseUIWindowView.InitInteractButton(interact);
        }

        public void SetActiveInteractButton(bool isActive)
        {
            _baseUIWindowView.SetActiveInteractButton(isActive);
        }

        private void SetHp(int hp)
        {
            _healthBarView.SetHp(hp);
        }
    }
}
