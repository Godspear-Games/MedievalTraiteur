using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance = null;
    private float _shiftTime = 0f;
    
    private float _timeLeft;
    
    private bool _timerStarted = false;
    private bool _timerPaused = false;

    private int _roundedTime;
    
    private void Awake()
    {
        Instance = this;
    }

    public void SetShiftTime(float time)
    {
        _shiftTime = time;
        _timeLeft = time;
        //round up and save in _roundedTime
        _roundedTime = Mathf.CeilToInt(_shiftTime);
    }
    
    public void StartTimer()
    {
        _timerStarted = true;
    }

    void Update()
    {
        if (_timerStarted && !_timerPaused)
        {
            //decrease time
            _timeLeft -= Time.deltaTime;
            
            //if current rounded time is not the same as the previous rounded time, update the timer
            if (_roundedTime != Mathf.CeilToInt(_timeLeft))
            {
                _roundedTime = Mathf.CeilToInt(_timeLeft);
                EventManager.Instance.UpdateTimer(_roundedTime, _shiftTime);
            }
            
            //if time is up, stop timer
            if (_timeLeft <= 0)
            {
                StopTimer();
                GameLoopManager.Instance.TimerEnded();
                EventManager.Instance.GameEnd();
            }
        }
    }

    public void PauseTimer()
    {
        _timerPaused = true;
    }
    
    public void UnpauseTimer()
    {
        _timerPaused = false;
    }

    public void StopTimer()
    {
        _timerStarted = false;
    }
}
