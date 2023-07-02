using System;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class BaseUIWindowView : UIView
    {
        [SerializeField] private ExitInfoView _exitInfoViewPrefab;
        [Space]
        [SerializeField] private TimerController _timerController;
        [Space]
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _interactButton;
        [Space]
        [SerializeField] private Button _exitInfosButton;
        [SerializeField] private VerticalLayoutGroup _exitInfosPanel;

        private void Start()
        {
            _inventoryButton.onClick.AddListener(UIMainController.Instance.OpenInventoryUI);
            _exitInfosButton.onClick.AddListener(ChangeActivityExitInfosPanel);
            _exitInfosPanel.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _inventoryButton.onClick.RemoveAllListeners();
            _exitInfosButton.onClick.RemoveAllListeners();
        }

        public void Init(List<string> exitNames, TimeSpan remainingTime)
        {
            UpdateExitInfosPanel(exitNames);
            _timerController.Init(remainingTime);
            SetActiveInteractButton(false);
        }

        public override void Hide()
        {
            base.Hide();
            _exitInfosPanel.gameObject.SetActive(false);
        }

        public void InitInteractButton(IInteract interact)
        {
            _interactButton.onClick.RemoveAllListeners();
            _interactButton.onClick.AddListener(interact.Interact);
        }

        public void SetActiveInteractButton(bool canInteract)
        {
            _interactButton.gameObject.SetActive(canInteract);
        }
        
        private void UpdateExitInfosPanel(List<string> exitNames)
        {
            var panel = _exitInfosPanel.gameObject;
            while (panel.transform.childCount > 0)
            {
                Destroy(panel.transform.GetChild(0)); 
            }

            foreach (var exitName in exitNames)
            {
               var exitInfo = Instantiate(_exitInfoViewPrefab, panel.transform);
               exitInfo.UpdateInfo(exitName);
            }
        }

        private void ChangeActivityExitInfosPanel()
        {
            _exitInfosPanel.gameObject.SetActive(!_exitInfosPanel.gameObject.activeSelf);
        }
    }
}
