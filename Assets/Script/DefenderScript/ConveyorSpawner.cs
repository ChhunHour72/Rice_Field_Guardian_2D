using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
     public Transform[] spawnpoints;
     public float minSecond = 1f;
     public float maxSecond = 15f;

    // Drag your 3 guardian prefabs here
    public GameObject[] guardians;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(minSecond, maxSecond);
            yield return new WaitForSeconds(delay);
            SpawnDefender();
        }
    }

    void SpawnDefender()
    {
        int spawnIndex = Random.Range(0, spawnpoints.Length);
        int guardianIndex = Random.Range(0, guardians.Length);

        Instantiate(
            guardians[guardianIndex],
            spawnpoints[spawnIndex].position,
            Quaternion.identity,
            spawnpoints[spawnIndex]
        );
    }   
}
