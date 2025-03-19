using UnityEngine;

public class Pheonix : MonoBehaviour
{
    public Transform window1; 
    public Transform window2; 
    public float speed = 5f; 
    public float waitTime = 2f; 

    private Transform target; 
    private bool isWaiting = false; 

    void Start()
    {
        target = window1;
    }

    void Update()
    {
        if (!isWaiting)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAndSwitchTarget());
        }
    }

    System.Collections.IEnumerator WaitAndSwitchTarget()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitTime);

        target = target == window1 ? window2 : window1;

        isWaiting = false;
    }
}