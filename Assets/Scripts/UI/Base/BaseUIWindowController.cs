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
            PlayerSaveLoadManager.Instance.OnHpChanged += SetHp;
            SetHp(PlayerSaveLoadManager.Instance.GetHp());
        }

        private void OnDisable()
        {
            GameBus.Instance.OnLevelSet -= SetLevelInfo;
            PlayerSaveLoadManager.Instance.OnHpChanged -= SetHp;
        }

        public void UpdateInteractButton(IInteract interact)
        {
            _view.UpdateInteractButton(interact);
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
            var lastLevelData = PlayerSaveLoadManager.Instance.GetLastLevelData();
            var exits = GameBus.Instance.Level.GetEntryExits();
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
