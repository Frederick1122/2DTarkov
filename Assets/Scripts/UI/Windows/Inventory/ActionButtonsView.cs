using System;
using Base.MVC;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class ActionButtonsView : UIView
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

        public override void UpdateView(UIModel uiModel)
        {
            var castData = (ActionButtonsModel) uiModel;
            
            _useButton.gameObject.SetActive(castData.isActiveUseButton);
            _equipButton.gameObject.SetActive(castData.isActiveEquipButton);
            _divideButton.gameObject.SetActive(castData.isActiveDivideButton);
            _dropButton.gameObject.SetActive(castData.isActiveDropButton);
            _moveButton.gameObject.SetActive(castData.isActiveMoveToStorageButton);
            
            base.UpdateView(uiModel);
        }
    }

    public class ActionButtonsModel : UIModel
    {
        public bool isActiveUseButton;
        public bool isActiveEquipButton;
        public bool isActiveDivideButton;
        public bool isActiveDropButton;
        public bool isActiveMoveToStorageButton;
        
        public ActionButtonsModel()
        {
            isActiveUseButton = false;
            isActiveEquipButton = false;
            isActiveDivideButton = false;
            isActiveDropButton = false;
            isActiveMoveToStorageButton = false;
        }
   
        public ActionButtonsModel(bool isActiveUseButton = false, bool isActiveEquipButton = false,
            bool isActiveDivideButton = false, bool isActiveDropButton = false, bool isActiveMoveToStorageButton = false)
        {
            this.isActiveUseButton = isActiveUseButton;
            this.isActiveEquipButton = isActiveEquipButton;
            this.isActiveDivideButton = isActiveDivideButton;
            this.isActiveDropButton = isActiveDropButton;
            this.isActiveMoveToStorageButton = isActiveMoveToStorageButton;
        }
    }
}
