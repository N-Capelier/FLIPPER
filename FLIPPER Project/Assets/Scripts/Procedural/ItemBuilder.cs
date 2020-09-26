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
    [SerializeField] float itemSpawnTime = 5f;
    [Range(0f, 1f)]
    [SerializeField] float decreaseRate = 0.1f;
    [Range(0f, 20f)]
    [SerializeField] float cooldownLimit = 2f;

    private void Awake()
    {
        itemTimer = new Clock(itemSpawnTime);
    }

    private void Update()
    {
        if (itemTimer.onFinish)
        {
            SpawnItem();
            if(itemSpawnTime > cooldownLimit)
            {
                itemSpawnTime -= decreaseRate * WorldManager.Instance.difficulty;
            }
            itemTimer.SetTime(itemSpawnTime);
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
