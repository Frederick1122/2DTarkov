using System;
using System.Collections.Generic;
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
            var lastLevelData = Player.Instance.GetLastLevelData();
            var exits = GameBus.Instance.GetLevel().GetEntryExits();
            var exitNames = new List<string>();
            foreach (var exitIndex in lastLevelData.exitIndexes)
            {
                exitNames.Add(exits[exitIndex].GetName());
            }
            
            _baseUIWindowView.Init(exitNames, lastLevelData.lastRemainingTime);
            
            SetHp(Player.Instance.GetHp());
        }

        private void OnEnable()
        {
            Player.Instance.OnHpChanged += SetHp;
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
