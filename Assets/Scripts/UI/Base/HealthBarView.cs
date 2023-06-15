using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _slider.maxValue = 100;
    }

    public void SetHp(int hp)
    {
        _slider.value = hp;
        _text.text = hp.ToString();
    }
}
