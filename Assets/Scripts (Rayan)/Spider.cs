using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private GameObject Butterfly;
    private float speed = Screen.height * 0.75f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Butterfly.transform.position, speed * Time.deltaTime);
        transform.up = Butterfly.transform.position - transform.position;
    }
}
