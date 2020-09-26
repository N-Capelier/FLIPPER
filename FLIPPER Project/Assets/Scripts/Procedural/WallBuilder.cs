using System.Collections;
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
    [SerializeField] float wallSpawnTimer = 5f;

    private void Awake()
    {
        wallTimer = new Clock(wallSpawnTimer);
    }

    private void Update()
    {
        if(wallTimer.onFinish)
        {
            SpawnWall();
            wallTimer.SetTime(1/(wallSpawnTimer * WorldManager.Instance.difficulty));
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
