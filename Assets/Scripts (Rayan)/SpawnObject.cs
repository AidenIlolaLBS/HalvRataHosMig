using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] public GameObject Prefab;
    [SerializeField] float spawnDistance = 10;

    public void SpawnPrefab()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        Instantiate(Prefab, spawnPos, playerRotation);
    }
}
