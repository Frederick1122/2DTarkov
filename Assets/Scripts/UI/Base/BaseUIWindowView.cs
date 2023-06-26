using System;
using System.Collections.Generic;
using Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class BaseUIWindowView : MonoBehaviour
    {
        [SerializeField] private ExitInfoView _exitInfoViewPrefab;
        [Space]
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _interactButton;
        [SerializeField] private TimerView _timer;
        [SerializeField] private Button _exitInfosButton;
        [SerializeField] private VerticalLayoutGroup _exitInfosPanel;

        private void OnEnable()
        {
            _inventoryButton.onClick.AddListener(UIMainController.Instance.OpenInventoryUI);
            _exitInfosButton.onClick.AddListener(ChangeActivityExitInfosPanel);
        }

        private void OnDisable()
        {
            _inventoryButton.onClick.RemoveAllListeners();
            _exitInfosButton.onClick.RemoveAllListeners();
        }

        public void Init(List<string> exitNames, TimeSpan remainingTime)
        {
            SetActiveInteractButton(false);
            UpdateExitInfosPanel(exitNames);
            _timer.Init(remainingTime);
        }

        public void InitInteractButton(IInteract interact)
        {
            _interactButton.onClick.RemoveAllListeners();
            
            _interactButton.onClick.AddListener(interact.Interact);
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

        public void SetActiveInteractButton(bool canInteract)
        {
            _interactButton.gameObject.SetActive(canInteract);
        }

        public void ChangeActivityExitInfosPanel()
        {
            _exitInfosPanel.gameObject.SetActive(_exitInfosPanel.gameObject.activeSelf);
        }
    }
}
