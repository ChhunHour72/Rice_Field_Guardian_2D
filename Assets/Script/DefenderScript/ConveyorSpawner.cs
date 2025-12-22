using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;

    public GameObject guardian;

    private void Start()
    {
        InvokeRepeating("SpawnEnermy", 2, 5);
    }

    void SpawnEnermy()
    {
        int r = Random.Range(0, spawnpoints.Length);
        GameObject myEnermy = Instantiate(guardian, spawnpoints[r].position, Quaternion.identity);
    }
}
