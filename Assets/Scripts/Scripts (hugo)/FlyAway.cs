using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyAway : MonoBehaviour
{
    private Rigidbody rb;
    private Coroutine flyingCoroutine;
    public float forceStrength = 10f;
    public float minInterval = 0.5f;
    public float maxInterval = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Call this method to start flying
    public void StartFlying()
    {
        if (rb != null && flyingCoroutine == null)
        {
            flyingCoroutine = StartCoroutine(ChangeDirectionRandomly());
            Debug.Log("Cube B started flying in random directions.");
        }
    }

    private IEnumerator ChangeDirectionRandomly()
    {
        while (true)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;

            rb.AddForce(randomDirection * forceStrength, ForceMode.Impulse);

            float randomWaitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(randomWaitTime);
        }
    }
}
