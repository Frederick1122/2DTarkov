using System;
using System.Collections;
using UnityEngine;

public class TimerController : UIController<TimerView, TimerModel>
{
    private TimeSpan _remainingTime;
    
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine;
    
    private void OnDisable()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
        }
    }

    private void OnApplicationQuit()
    {
        Player.Instance.SetLastRemainingTime(_remainingTime);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        Player.Instance.SetLastRemainingTime(_remainingTime);
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
        var timerData = new TimerModel();
        while (_remainingTime.TotalSeconds > 0)
        {
            timerData.remainingTime = _remainingTime;
            _view.UpdateView(timerData);
            yield return new WaitForSeconds(1f);
            _remainingTime -= TimeSpan.FromSeconds(1);
        }
    }
}
