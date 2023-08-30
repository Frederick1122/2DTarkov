using UI;
using UnityEngine;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        var uiMainController = UIMainController.Instance;
        
        if(uiMainController != null)
            _button.onClick.AddListener(() => uiMainController.OpenBaseUI());
    }
}
