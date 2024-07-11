using System;
using System.Collections.Generic;
using Base;
using Base.MVC;
using Managers.SaveLoadManagers;
using UnityEngine;

namespace UI.Base
{
    public class BaseUIWindowController : WindowController
    {
        [SerializeField] private HealthBarView _healthBarView;

        public override void Init()
        {
            GameBus.Instance.OnLevelSet += SetLevelInfo;
            PlayerSaveLoadManager.Instance.OnHpChanged += SetHp;
            SetHp(PlayerSaveLoadManager.Instance.GetHp());

            base.Init();
        }

        public override void Terminate()
        {
            GameBus.Instance.OnLevelSet -= SetLevelInfo;
            PlayerSaveLoadManager.Instance.OnHpChanged -= SetHp;
            base.Terminate();
        }

        public void UpdateInteractButton(IInteract interact)
        {
            GetView<BaseUIWindowView>().UpdateInteractButton(interact);
        }

        public void SetActiveInteractButton(bool isActive)
        {
            GetView<BaseUIWindowView>().SetActiveInteractButton(isActive);
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
            
            GetView<BaseUIWindowView>().Init(exitNames, newTimeSpan);
        }

        protected override UIModel GetViewData()
        {
            return new UIModel();
        }
    }
}
