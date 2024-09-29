using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserClockSetter : MonoBehaviour
{
    DateTime newTime;

    public bool settingTime;
    public bool writingTime;

    [SerializeField]
    GameObject buttonSetTime;
    [SerializeField]
    GameObject buttonEndSettingTime;
    [SerializeField]
    GameObject buttonEnterTime;

    [SerializeField]
    ToggleSystem toggleSystem;

    [SerializeField]
    TMP_InputField inputField;

    ClockAnimation clockAnimation;
    void Start()
    {
        clockAnimation = GameObject.Find("RoundClock").GetComponent<ClockAnimation>();

    }

    void Update()
    {
        if (settingTime && !writingTime)
        {
            float hourHandleRot = clockAnimation.hourHand.transform.eulerAngles.z;
            float minHandleRot = clockAnimation.minuteHand.transform.eulerAngles.z;

            int newMin = (int)Math.Abs((minHandleRot / 6) - 60);
            int newHour = (int)Math.Abs((hourHandleRot / 30) - 12);

            DateTime currentTime = clockAnimation.GetCurrentTime();
            if (toggleSystem.isAM)
            {
                newTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                    newHour, newMin, currentTime.Second, currentTime.Millisecond, System.DateTimeKind.Utc);
            }
            else
            {
                newTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day,
                    newHour + 12, newMin, currentTime.Second, currentTime.Millisecond, System.DateTimeKind.Utc);

            }

            clockAnimation.UpdateDigitalClock(newTime);
        }
    }
    public void SetUserTime()
    {
        if (!settingTime)
        {
            settingTime = true;
            toggleSystem.gameObject.SetActive(true);

            buttonSetTime.SetActive(false);
            buttonEndSettingTime.SetActive(true);
            buttonEnterTime.SetActive(true);

        }

    }
    public void StopSettingUserTime()
    {
        if (settingTime)
        {
            settingTime = false;
            toggleSystem.gameObject.SetActive(false);
            inputField.gameObject.SetActive(false);

            buttonSetTime.SetActive(true);
            buttonEndSettingTime.SetActive(false);
            buttonEnterTime.SetActive(false);
            clockAnimation.SetClock(newTime);
        }
    }
    public void UserInputTime()
    {
        Regex regex = new Regex(@"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]:[0-5][0-9]$");
        string input = inputField.text;
        Debug.Log(input);

        if (regex.IsMatch(input))
        {
            newTime = DateTime.ParseExact(input, "HH:mm:ss", CultureInfo.InvariantCulture);
            clockAnimation.SetClock(newTime);
            StopSettingUserTime();
        }
        else
        {
            Debug.Log("WrongFormat");
        }
        writingTime = false;
    }
    public void EnterTime()
    {
        buttonEnterTime.SetActive(false);
        inputField.gameObject.SetActive(true);
        writingTime = true;
    }
}
