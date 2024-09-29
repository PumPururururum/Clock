using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ClockAnimation : MonoBehaviour
{
    public GameObject hourHand;
    public GameObject minuteHand;
    public GameObject secondHand;

    [SerializeField]
    TMP_Text digitalClock;

    DateTime currentTime;

    float bTime;
    bool startClock = false;

    UserClockSetter clockSetter;
    private void Start()
    {
        clockSetter = GameObject.Find("ClockSetter").GetComponent<UserClockSetter>();
    }

    void Update()
    {
        if (!clockSetter.settingTime)
        {
            bTime += Time.deltaTime;
            CountTime(Time.deltaTime);
            if (startClock && bTime >=1)
            {
                UpdateClock();
                bTime = 0;
            }
        }
    }

    public void UpdateClock()
    {
        int sec = currentTime.Second;
        int min = currentTime.Minute;
        int hour = currentTime.Hour;

        digitalClock.text = hour + ":" + min + ":" + sec;
        float minDistance = (float)(sec) / 60f;
        float hourDistance = (float)(min) / 60f;

        secondHand.transform.DORotate(new Vector3(0f,0f,-6*sec),1, RotateMode.Fast);
        minuteHand.transform.DORotate(new Vector3(0f, 0f, -6 * (min + minDistance)), 1, RotateMode.Fast);
        hourHand.transform.DORotate(new Vector3(0f, 0f, (hour + hourDistance) * 360 / 12 * -1), 1, RotateMode.Fast);
    }
    public void SetClock(DateTime time)
    {
        currentTime = time;
        startClock = true;
    }

    private void CountTime(float time)
    {
        currentTime = currentTime.AddSeconds((double)time);
    }
    public DateTime GetCurrentTime()
    {
        return currentTime;
    }
    public void UpdateDigitalClock(DateTime time)
    {
        int sec = time.Second;
        int min = time.Minute;
        int hour = time.Hour;

        digitalClock.text = hour + ":" + min + ":" + sec;
    }

}
