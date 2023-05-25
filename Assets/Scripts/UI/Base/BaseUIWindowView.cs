using Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class BaseUIWindowView : MonoBehaviour
    {
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _interactButton;

        public void Init()
        {
            _inventoryButton.onClick.AddListener(() => UIMainController.Instance.OpenInventoryUI());
            SetActiveInteractButton(false);
        }

        public void InitInteractButton(IInteract interact)
        {
            _interactButton.onClick.AddListener(interact.Interact);
        }

        public void SetActiveInteractButton(bool canInteract)
        {
            _interactButton.gameObject.SetActive(canInteract);
        }
    }
}
