using System;
using TMPro;
using UnityEngine;

public class TimerView : UIView
{
    [SerializeField] private TMP_Text _text;

    public void GenerateTimeText(TimeSpan remainingTime)
    {
        _text.text =
            remainingTime.Minutes.ToString("D2")  +
            ":" + remainingTime.Seconds.ToString("D2"); //$"{SetTimeText(remainingTime.Minutes)}:{SetTimeText(remainingTime.Seconds)}";
    }
}
