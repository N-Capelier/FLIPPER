﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBuilder : MonoBehaviour
{
    Clock wallTimer = null;

    public Transform[] leftSpawnPoints = null;
    public Transform[] rightSpawnPoints = null;
    public GameObject[] walls = null;

    [Header("Timings")]
    [Range(1f, 20f)]
    [SerializeField] float wallSpawnCooldown = 5f;
    [Range(0f, 1f)]
    [SerializeField] float decreaseRate = 0.1f;
    [Range(0f, 20f)]
    [SerializeField] float cooldownLimit = 2f;

    private void Awake()
    {
        wallTimer = new Clock(wallSpawnCooldown);
    }

    private void Update()
    {
        if(wallTimer.onFinish)
        {
            SpawnWall();
            if (wallSpawnCooldown > cooldownLimit)
            {
                wallSpawnCooldown -= decreaseRate * WorldManager.Instance.difficulty;

            }
            wallTimer.SetTime(wallSpawnCooldown);
        }
    }

    void SpawnWall()
    {
        if (WorldManager.Instance.isLevelDesignSymetrical)
        {
            int spawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int wallIndex = Random.Range(0, walls.Length);

            Instantiate(walls[wallIndex], leftSpawnPoints[spawnPointIndex]);
            Instantiate(walls[wallIndex], rightSpawnPoints[spawnPointIndex]);
        }
        else
        {
            int leftSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int rightSpawnPointIndex = Random.Range(0, rightSpawnPoints.Length);

            int leftWallIndex = Random.Range(0, walls.Length);
            int rightWallIndex = Random.Range(0, walls.Length);

            Instantiate(walls[leftWallIndex], leftSpawnPoints[leftSpawnPointIndex]);
            Instantiate(walls[rightWallIndex], rightSpawnPoints[rightSpawnPointIndex]);
        }
    }
}
