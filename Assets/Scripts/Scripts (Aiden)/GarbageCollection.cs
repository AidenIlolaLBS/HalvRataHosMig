using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageCollection : MonoBehaviour
{
    GameObject spawnPoint;
    /// <summary>
    /// 0: Top left corner of area. !: Bottom right corner of area.
    /// </summary>
    GameObject[] playableArea = new GameObject[2];

    Rigidbody rigidbody;
    private void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnLocation");
        playableArea[0] = GameObject.FindGameObjectWithTag("PlayableArea1");
        playableArea[1] = GameObject.FindGameObjectWithTag("PlayableArea2");

        rigidbody = transform.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (playableArea[0] != null)
        {
            if (transform.position.x < playableArea[0].transform.position.x ||
            transform.position.y > playableArea[0].transform.position.y ||
            transform.position.z > playableArea[0].transform.position.z)
            {
                SetPos();
            }
            else if (transform.position.x > playableArea[1].transform.position.x ||
                transform.position.y < playableArea[1].transform.position.y ||
                transform.position.z < playableArea[1].transform.position.z)
            {
                SetPos();
            }
        }        
    }

    void SetPos()
    {
        transform.position = spawnPoint.transform.position;
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
