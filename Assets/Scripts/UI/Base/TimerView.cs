using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerView : UIView
{
    [SerializeField] private TMP_Text _text;
    
    private TimeSpan _remainingTime;
    
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine;
    
    public void Init(TimeSpan remainingTime)
    {
        _remainingTime = remainingTime;
        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (_remainingTime.TotalSeconds > 0)
        {
            _text.text = $"{_remainingTime.Minutes} : {_remainingTime.Seconds}";
            yield return _second;
            _remainingTime -= TimeSpan.FromSeconds(1);
        }
    }
}
