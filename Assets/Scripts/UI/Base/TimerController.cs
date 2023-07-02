using System;
using System.Collections;
using UnityEngine;

public class TimerController : UIController<TimerView>
{
    private TimeSpan _remainingTime;
    
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine;
    
    private void OnDisable()
    {
        Player.Instance.SetLastRemainingTime(_remainingTime);
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
        }
    }
    
    public void Init(TimeSpan remainingTime)
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
        }
        _remainingTime = remainingTime;
        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (_remainingTime.TotalSeconds > 0)
        {
            _view.GenerateTimeText(_remainingTime);
            yield return new WaitForSeconds(1f);
            _remainingTime -= TimeSpan.FromSeconds(1);
        }
    }
}
