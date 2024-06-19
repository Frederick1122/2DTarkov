using System;
using System.Collections.Generic;
using Base;
using UnityEngine;

namespace UI.Base
{
    public class BaseUIWindowController : WindowController<BaseUIWindowView, BaseUIWindowModel>
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private void Start()
        {
            GameBus.Instance.OnLevelSet += SetLevelInfo;
            PlayerManager.Instance.OnHpChanged += SetHp;
            SetHp(PlayerManager.Instance.GetHp());
        }

        private void OnDisable()
        {
            GameBus.Instance.OnLevelSet -= SetLevelInfo;
            PlayerManager.Instance.OnHpChanged -= SetHp;
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
            var lastLevelData = PlayerManager.Instance.GetLastLevelData();
            var exits = GameBus.Instance.GetLevel().GetEntryExits();
            var exitNames = new List<string>();
            foreach (var exitIndex in lastLevelData.exitIndexes)
            {
                exitNames.Add(exits[exitIndex].GetName());
            }

            var newTimeSpan = new TimeSpan(0, lastLevelData.lastRemainingMinutes,
                lastLevelData.lastRemainingSeconds);
            
            _view.Init(exitNames, newTimeSpan);
        }
    }
}
