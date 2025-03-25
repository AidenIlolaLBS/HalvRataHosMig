using UnityEngine;
using System.Collections;

public class TimerCube : MonoBehaviour, IPickupable
{
    public float timerDuration = 5f;
    private float timer;
    private bool isTimerActive = false;

    void Start()
    {
        timer = timerDuration;
    }

    public void OnPickup()
    {
        if (!isTimerActive)
        {
            StartTimer();
        }
    }

    private void StartTimer()
    {
        isTimerActive = true;
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        TimerEnded();
    }

    private void TimerEnded()
    {
        isTimerActive = false;
        Debug.Log("Timer has ended! Perform action here.");
    }
}
