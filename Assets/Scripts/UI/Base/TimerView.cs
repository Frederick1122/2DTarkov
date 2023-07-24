using System;
using TMPro;
using UnityEngine;

public class TimerView : UIView<TimerModel>
{
    [SerializeField] private TMP_Text _text;
    
    public override void UpdateView(TimerModel uiModel)
    {
        _text.text =
            uiModel.remainingTime.Minutes.ToString("D2")  +
            ":" + uiModel.remainingTime.Seconds.ToString("D2");
        base.UpdateView(uiModel);
    }
}

public class TimerModel : UIModel
{
    public TimeSpan remainingTime = new TimeSpan();
}