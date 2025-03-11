using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool open = false;
    public bool Open
    {
        get { return open; }
    }
    public float doorAngle = 90;
    public void InteractDoor()
    {
        if (open)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
            open = false;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y - doorAngle, 0);
            open = true;
        }
    }
}
