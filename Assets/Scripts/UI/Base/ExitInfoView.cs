using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExitInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    public void UpdateInfo(string exitName)
    {
        text.text = exitName;
    }
}
