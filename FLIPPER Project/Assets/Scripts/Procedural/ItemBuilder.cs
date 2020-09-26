using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    Clock itemTimer = null;

    public Transform[] leftSpawnPoints = null;
    public Transform[] rightSpawnPoints = null;
    public GameObject[] items = null;

    [Header("Timings")]
    [Range(1f, 20f)]
    [SerializeField] float itemSpawnTimer = 5f;

    private void Awake()
    {
        itemTimer = new Clock(itemSpawnTimer);
    }

    private void Update()
    {
        if (itemTimer.onFinish)
        {
            SpawnItem();
            itemTimer.SetTime(1 / (itemSpawnTimer * WorldManager.Instance.difficulty));
        }
    }

    void SpawnItem()
    {
        if (WorldManager.Instance.isLevelDesignSymetrical)
        {
            int spawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int itemIdex = Random.Range(0, items.Length);

            Instantiate(items[itemIdex], leftSpawnPoints[spawnPointIndex]);
            Instantiate(items[itemIdex], rightSpawnPoints[spawnPointIndex]);
        }
        else
        {
            int leftSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int rightSpawnPointIndex = Random.Range(0, rightSpawnPoints.Length);

            int leftItemIndex = Random.Range(0, items.Length);
            int rightItemIndex = Random.Range(0, items.Length);

            Instantiate(items[leftItemIndex], leftSpawnPoints[leftSpawnPointIndex]);
            Instantiate(items[rightItemIndex], rightSpawnPoints[rightSpawnPointIndex]);
        }
    }
}
