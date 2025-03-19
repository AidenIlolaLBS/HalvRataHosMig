using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float startTime = 15f; // Set the starting time in seconds
    private float timeRemaining;
    private bool timerIsRunning = false;
    public GameObject Object;

    public TextMeshProUGUI timerText; // Drag your UI text element here in the Inspector

    void Start()
    {
        // Initialize the timer
        timeRemaining = startTime;
        StartTimer();
    }

    void Update()
    {
        // Only update the timer if it's running
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                TimerEnded();
            }
        }
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay = Mathf.Clamp(timeToDisplay, 0, Mathf.Infinity);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        // Add any specific actions when the timer ends
        Debug.Log("Time's up!");
        Object.GetComponent<ObjectMoving>().shouldMove = false;
    }
}
