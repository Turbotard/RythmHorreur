using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;
    public float spawnDistance = 15f;
    public float minY = -3f;
    public float maxY = 3f;
    public float minimumGap = 2f;
    public float minDistanceToMove = 5f;
    public float maxDistanceToMove = 10f;

    private Vector3 lastPlayerPosition;
    private Vector2 lastSpawnPosition;
    private float distanceToMove;

    void Start()
    {
        lastPlayerPosition = player.position;
        lastSpawnPosition = new Vector2(-Mathf.Infinity, 0);
        RandomizeDistanceToMove();
    }

    void Update()
    {
        if (Vector3.Distance(lastPlayerPosition, player.position) >= distanceToMove)
        {
            SpawnObstacle();
            lastPlayerPosition = player.position;
            RandomizeDistanceToMove();
        }
    }

    void SpawnObstacle()
    {
        Vector2 spawnPosition;

        do
        {
            spawnPosition = new Vector2(player.position.x + spawnDistance, -2.41f);
        } while (Vector2.Distance(lastSpawnPosition, spawnPosition) < minimumGap);

        Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        lastSpawnPosition = spawnPosition;
    }

    void RandomizeDistanceToMove()
    {
        distanceToMove = Random.Range(minDistanceToMove, maxDistanceToMove);
    }
}