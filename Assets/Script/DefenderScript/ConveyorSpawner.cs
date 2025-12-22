using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    public Transform[] spawnpoints;

    public GameObject guardian;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(10f, 25f);
            yield return new WaitForSeconds(delay);
            SpawnDefender();
        }
    }

    void SpawnDefender()
    {
        int r = Random.Range(0, spawnpoints.Length);
        GameObject myDefender = Instantiate(
            guardian,
            spawnpoints[r].position,
             Quaternion.identity,
            spawnpoints[r]  
        );
    }
}
