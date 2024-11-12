using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{
    float x;
    float y;
    float z;
    Vector3 pos;

    void Start()
    {
        x = Random.Range(0, 800);
        y = Random.Range(200, 400);
        z = 0;
        pos = new Vector3(x, y, z);
        transform.position = pos;
    }
}
