using System;
using Base.MVC;
using TMPro;
using UnityEngine;

public class TimerView : UIView //<TimerModel>
{
    [SerializeField] private TMP_Text _text;
    
    public override void UpdateView(UIModel uiModel)
    {
        var castData = (TimerModel) uiModel;
        
        _text.text =
            castData.remainingTime.Minutes.ToString("D2")  +
            ":" + castData.remainingTime.Seconds.ToString("D2");
        base.UpdateView(uiModel);
    }
}

public class TimerModel : UIModel
{
    public TimeSpan remainingTime = new TimeSpan();
}