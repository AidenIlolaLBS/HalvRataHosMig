using UnityEngine;

public class BirdFly : MonoBehaviour
{
    public Transform window1; // F�rsta positionen (f�nster 1)
    public Transform window2; // Andra positionen (f�nster 2)
    public float speed = 5f; // Hastigheten f�geln flyger med
    public float waitTime = 2f; // Tid f�geln v�ntar vid ett f�nster innan den flyger vidare

    private Transform target; // Nuvarande m�lposition
    private bool isWaiting = false; // Om f�geln v�ntar just nu

    void Start()
    {
        // B�rja med att s�tta m�let till det f�rsta f�nstret
        target = window1;
    }

    void Update()
    {
        // Flytta f�geln mot m�let om den inte v�ntar
        if (!isWaiting)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        // Flytta f�geln mot m�let
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Kontrollera om f�geln har n�tt m�let
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Byt m�l och starta v�ntan
            StartCoroutine(WaitAndSwitchTarget());
        }
    }

    System.Collections.IEnumerator WaitAndSwitchTarget()
    {
        isWaiting = true;

        // V�nta den angivna tiden
        yield return new WaitForSeconds(waitTime);

        // Byt m�l
        target = target == window1 ? window2 : window1;

        isWaiting = false;
    }
}