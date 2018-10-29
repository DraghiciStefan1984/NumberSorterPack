using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance = null;
    public float GameTimer { get; set; }
    public bool IsGameRunning { get; set; }
    TextMeshProUGUI timerText;
    float startTime;
    float endTime;
    int seconds;
    int minutes;

    // Use this for initialization
    void Awake()
    {
        GetInstance();
        IsGameRunning = true;
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update ()
    {
        ManageTimer();
	}

    public TextMeshProUGUI GetTimerText()
    {
        return timerText;
    }

    void GetInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ManageTimer()
    {
        if(IsGameRunning)
        {
            GameTimer=Time.time - startTime;
            //GameTimer += Time.deltaTime;
            seconds = (int)(GameTimer % 60);
            minutes = (int)(GameTimer / 60) % 60;
            string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
            timerText.text = timerString;
        }
    }

    public void StopTimer()
    {
        IsGameRunning = false;
        GameTimer = Time.time - startTime;
    }
}
