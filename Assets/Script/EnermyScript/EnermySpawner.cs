using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermySpawner : MonoBehaviour
{
    public Transform[] spawnpoints;

    public GameObject ant;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

     IEnumerator SpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(10f, 20f);  // Random delay
            yield return new WaitForSeconds(delay);

            SpawnEnermy();
        }
    }


    void SpawnEnermy()
    {
        int r = Random.Range(0, spawnpoints.Length);
        GameObject myEnermy = Instantiate(ant, spawnpoints[r].position, Quaternion.identity);
    }
}
