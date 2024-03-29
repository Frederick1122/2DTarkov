using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ActionButtonsView : MonoBehaviour
    {
        public event Action OnUseAction;
        public event Action OnEquipAction;
        public event Action OnDivideAction;
        public event Action OnDropAction;
        public event Action OnMoveAction;
    
        [Space] [Header("Action Buttons")]
        [SerializeField] private Button _useButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private Button _divideButton;
        [SerializeField] private Button _dropButton;
        [SerializeField] private Button _moveButton;

        private void Start()
        {
            _useButton.onClick.AddListener(() => OnUseAction?.Invoke());
            _equipButton.onClick.AddListener(() => OnEquipAction?.Invoke());
            _divideButton.onClick.AddListener(() => OnDivideAction?.Invoke());
            _dropButton.onClick.AddListener(() => OnDropAction?.Invoke());
            _moveButton.onClick.AddListener(() => OnMoveAction?.Invoke());
        }

        public void SetActiveButtons(bool isActiveUseButton = false, bool isActiveEquipButton = false,
            bool isActiveDivideButton = false, bool isActiveDropButton = false, bool isMoveToStorageButton = false)
        {
            _useButton.gameObject.SetActive(isActiveUseButton);
            _equipButton.gameObject.SetActive(isActiveEquipButton);
            _divideButton.gameObject.SetActive(isActiveDivideButton);
            _dropButton.gameObject.SetActive(isActiveDropButton);
            _moveButton.gameObject.SetActive(isMoveToStorageButton);
        }
    }
}
