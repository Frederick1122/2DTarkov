using UI;
using UnityEngine;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        var uiMainController = UIManager.Instance;
        
        if(uiMainController != null)
            _button.onClick.AddListener(() => uiMainController.OpenBaseUI());
    }
}
