using UI;
using UnityEngine;
using UnityEngine.UI;

public class CloseWindow : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() => UIMainController.Instance.OpenBaseUI());
    }
}
