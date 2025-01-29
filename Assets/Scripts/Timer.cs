using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeRemaining = 0;
    private bool timerIsRunning = false;
    [SerializeField] private TextMeshProUGUI timeText;
    
    void Start()
    {
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }
    public int GetTime()
    {
        int totalSeconds = Mathf.FloorToInt(timeRemaining);
        Debug.Log("Total seconds: " + totalSeconds);
        return totalSeconds;
    }
}
