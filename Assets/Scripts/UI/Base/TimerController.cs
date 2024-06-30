using System;
using System.Collections;
using Managers.SaveLoadManagers;
using Base.MVC;
using UnityEngine;

public class TimerController : UIController //<TimerView, TimerModel>
{
    private TimeSpan _remainingTime;
    
    private YieldInstruction _second = new WaitForSeconds(1f);
    private Coroutine _timerRoutine;

    private TimerModel _data = new();
    
    private void OnDisable()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerSaveLoadManager.Instance.SetLastRemainingTime(_remainingTime);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        PlayerSaveLoadManager.Instance.SetLastRemainingTime(_remainingTime);
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
            UpdateView();
            yield return new WaitForSeconds(1f);
            _remainingTime -= TimeSpan.FromSeconds(1);
        }
    }

    protected override UIModel GetViewData()
    {
        _data.remainingTime = _remainingTime;
        return _data;
    }
}
