using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermySpawner : MonoBehaviour
{
    public Transform[] spawnpoints;
    public GameObject[] enemy;

    public float minTime = 5f;
    public float maxTime = 10f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            // Skip spawning while tutorial is active
            if (Dialogue.isTutorialActive)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }
            
            float delay = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(delay);

            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Random spawn point
        int spawnIndex = Random.Range(0, spawnpoints.Length);

        // Random enemy
        int enemyIndex = Random.Range(0, enemy.Length);

        Instantiate(
            enemy[enemyIndex],
            spawnpoints[spawnIndex].position,
            Quaternion.Euler(0f, 180f, 0f),
            spawnpoints[spawnIndex]
        );
    }
}
