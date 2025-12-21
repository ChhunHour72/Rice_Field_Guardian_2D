using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 2f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
    }

    void SpawnItem()
    {
        Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
    }
}
