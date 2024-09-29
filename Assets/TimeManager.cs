using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

public class TimeManager : MonoBehaviour
{
    public DateTime time;
    [System.Serializable]
    public class TimeJson
    {
        public string time;
        public string clocks;
    }

    private ClockAnimation clockAnimation;
    private void Start()
    {
        clockAnimation = GameObject.Find("RoundClock").GetComponent<ClockAnimation>();
        StartCoroutine(GetServerTime());
    }
    private IEnumerator GetServerTime()
    {
        string url = "https://corsproxy.io/?https://yandex.com/time/sync.json";
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Succesfully got time from server");

        }
        var text = request.downloadHandler.text;
        TimeJson serverTime = JsonUtility.FromJson<TimeJson>(text);
       

        Debug.Log(serverTime.time);
        time = ConvertTimeStampToDate(serverTime.time);
        SetClockTime();
        InvokeRepeating("StartVerify", 3600.0f, 3600.0f);

    }
    private IEnumerator VerifyTimeWithServer()
    {
        string url = "https://yandex.com/time/sync.json";
        UnityWebRequest request = UnityWebRequest.Get(url);

        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Succesfully got time from server");

        }
        var text = request.downloadHandler.text;
        TimeJson serverTime = JsonUtility.FromJson<TimeJson>(text);


        Debug.Log(serverTime.time);
        time = ConvertTimeStampToDate(serverTime.time);
        if (time != clockAnimation.GetCurrentTime())
        {
            Debug.Log("Changed time from " + clockAnimation.GetCurrentTime().ToString() + " to " + time.ToString());
            SetClockTime();
        }
    }
    private void StartVerify()
    {
        StartCoroutine(VerifyTimeWithServer());
    }
    private DateTime ConvertTimeStampToDate(string timeStamp)
    {
        long timeStampL = long.Parse(timeStamp);

        DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        result = result.AddMilliseconds(timeStampL).ToLocalTime();
        return result;
    }
    private void SetClockTime()
    {
        Debug.Log(time.ToString());
        clockAnimation.SetClock(time);
    } 
}
