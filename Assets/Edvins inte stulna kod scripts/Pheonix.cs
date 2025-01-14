using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public Transform window1; // Första positionen (fönster 1)
    public Transform window2; // Andra positionen (fönster 2)
    public float speed = 5f; // Hastigheten fågeln flyger med
    public float waitTime = 2f; // Tid fågeln väntar vid ett fönster innan den flyger vidare

    private Transform target; // Nuvarande målposition
    private bool isWaiting = false; // Om fågeln väntar just nu

    void Start()
    {
        // Börja med att sätta målet till det första fönstret
        target = window1;
    }

    void Update()
    {
        // Flytta fågeln mot målet om den inte väntar
        if (!isWaiting)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Flytta fågeln mot målet
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Kontrollera om fågeln har nått målet
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Byt mål och starta väntan
            StartCoroutine(WaitAndSwitchTarget());
        }
    }

    System.Collections.IEnumerator WaitAndSwitchTarget()
    {
        isWaiting = true;

        // Vänta den angivna tiden
        yield return new WaitForSeconds(waitTime);

        // Byt mål
        target = target == window1 ? window2 : window1;

        isWaiting = false;
    }
}