using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermySpawner : MonoBehaviour
{
    public Transform[] spawnpoints;

    public GameObject ant;

    private void Start()
    {
        InvokeRepeating("SpawnEnermy", 2, 5);
    }

    void SpawnEnermy()
    {
        int r = Random.Range(0, spawnpoints.Length);
        GameObject myEnermy = Instantiate(ant, spawnpoints[r].position, Quaternion.identity);
    }
}
