using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI displayedTime;
    private bool timerIsRunning;
    private TimerValue currentTimerValue = new();

    [System.Serializable]
    public class TimerValue
    {
        public float seconds;
        public float minutes;
    }

    private void Awake()
    {
        displayedTime = GetComponent<TextMeshProUGUI>();
    }

    private void OnGameEnd(object sender, EventArgs e)
    {
        StopTimer();
    }

    private void Start()
    {
        timerIsRunning = false;
        GameManager.Instance.OnGameEnd += OnGameEnd;
        ResetTimer();
        StartTimer(); //to i wywo³anie ResetTimer nie powinno byæ w tym miejscu tylko po wciœniêciu przycisku Start Game czy coœ
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            currentTimerValue.seconds += Time.deltaTime;

            if (currentTimerValue.seconds >= 59.6) 
            {
                currentTimerValue.minutes++;
                currentTimerValue.seconds = 0;
            }
            
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        displayedTime.text = string.Format("{0:00}:{1:00}", currentTimerValue.minutes, currentTimerValue.seconds);
    }

    void ResetTimer()
    {
        currentTimerValue.seconds = 0;
        currentTimerValue.minutes = 0;
    }

    public TimerValue GetCurrentTimerValue()
    {
        return currentTimerValue;
    }
}
