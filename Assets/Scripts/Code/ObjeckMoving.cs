using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    public float speed = 5f;
    public float moveRange = 5f;
    public bool shouldMove = true;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position; 
    }

    void Update()
    {
        MoveObject();
    }
   
    void MoveObject()
    {
        float movement = Mathf.PingPong(Time.time * speed, moveRange);
        if (shouldMove)
        {
            transform.position = new Vector3(startPos.x + movement, startPos.y, startPos.z);
        }
    }
}
