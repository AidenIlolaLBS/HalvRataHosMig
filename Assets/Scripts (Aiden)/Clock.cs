using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject hour;
    public GameObject minute;
    public GameObject second;

    public float timeChange = 1;

    // Update is called once per frame
    void Update()
    {
        second.transform.rotation *= Quaternion.Euler(0,0,-6 * timeChange * Time.deltaTime);
        minute.transform.rotation *= Quaternion.Euler(0,0,-0.1f * timeChange * Time.deltaTime);
        hour.transform.rotation *= Quaternion.Euler(0,0,-(0.1f / 24) * timeChange * Time.deltaTime);
    }
}
