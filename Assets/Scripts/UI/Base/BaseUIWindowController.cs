using System.Collections.Generic;
using Base;
using UnityEngine;

namespace UI.Base
{
    public class BaseUIWindowController : WindowController<BaseUIWindowView>
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private void Start()
        {
            GameBus.Instance.OnLevelSet += SetLevelInfo;
            Player.Instance.OnHpChanged += SetHp;
            SetHp(Player.Instance.GetHp());
        }

        private void OnDisable()
        {
            GameBus.Instance.OnLevelSet -= SetLevelInfo;
            Player.Instance.OnHpChanged -= SetHp;
        }

        public void InitInteractButton(IInteract interact)
        {
            _view.InitInteractButton(interact);
        }

        public void SetActiveInteractButton(bool isActive)
        {
            _view.SetActiveInteractButton(isActive);
        }

        private void SetHp(int hp)
        {
            _healthBarView.SetHp(hp);
        }

        private void SetLevelInfo(Level currentLevel)
        {
            var lastLevelData = Player.Instance.GetLastLevelData();
            var exits = GameBus.Instance.GetLevel().GetEntryExits();
            var exitNames = new List<string>();
            foreach (var exitIndex in lastLevelData.exitIndexes)
            {
                exitNames.Add(exits[exitIndex].GetName());
            }
            
            _view.Init(exitNames, lastLevelData.lastRemainingTime);
        }
    }
}
