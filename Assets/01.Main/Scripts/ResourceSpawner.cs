using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject treePrefab;
    public GameObject stonePrefab;
    public float spawnChance;

    [Header("Raycast Settings")]
    public float distanceBetweenChecks;
    public float heightOfCheck = 200f;
    public float rangeOfCheck = 200f;
    public LayerMask layerMask;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    private void Start()
    {
        SpawnTrees();
        SpawnStones();
    }

    void SpawnTrees()
    {
        for (float x = minPosition.x; x < maxPosition.x; x += distanceBetweenChecks)
        {
            for (float z = minPosition.y; z < maxPosition.y; z += distanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    if (spawnChance > Random.Range(0, 101))
                    {
                        Instantiate(treePrefab, hit.point, Quaternion.identity, transform);
                    }
                }
            }
        }
    }

    void SpawnStones()
    {
        for (float x = minPosition.x; x < maxPosition.x; x += 13f)
        {
            for (float z = minPosition.y; z < maxPosition.y; z += 13f)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightOfCheck, z), Vector3.down, out hit, rangeOfCheck, layerMask))
                {
                    if (spawnChance > Random.Range(0, 101))
                    {
                        Instantiate(stonePrefab, hit.point, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
