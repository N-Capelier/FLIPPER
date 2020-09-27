using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBuilder : MonoBehaviour
{
    Clock itemTimer = null;

    public Transform[] leftSpawnPoints = null;
    public Transform[] rightSpawnPoints = null;
    public Transform[] mergedSpawnPoints = null;
    public GameObject[] items = null;

    [Header("Timings")]
    [Range(1f, 20f)]
    [SerializeField] float itemSpawnTime = 5f;
    [Range(0f, 1f)]
    [SerializeField] float decreaseRate = 0.1f;
    [Range(0f, 20f)]
    [SerializeField] float cooldownLimit = 2f;

    int luckRate = 2;

    private void Awake()
    {
        itemTimer = new Clock(itemSpawnTime);
    }

    private void Update()
    {
        if (itemTimer.onFinish)
        {
            SpawnItem();
            if (itemSpawnTime > cooldownLimit)
            {
                itemSpawnTime -= decreaseRate * WorldManager.Instance.difficulty;
            }
            itemTimer.SetTime(itemSpawnTime);
        }
    }

    void SpawnItem()
    {
        if (WorldManager.Instance.isMerged)
        {
            SpawnItemMerged();
            return;
        }

        if (WorldManager.Instance.isLevelDesignSymetrical)
        {
            int spawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int itemIndex = Random.Range(0, items.Length);

            Instantiate(items[itemIndex], leftSpawnPoints[spawnPointIndex]);
            Instantiate(items[itemIndex], rightSpawnPoints[spawnPointIndex]);

            int luck = Random.Range(0, luckRate);
            if(luck == 0)
            {
                int luckSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
                if(luckSpawnPointIndex != spawnPointIndex)
                {
                    Instantiate(items[itemIndex], leftSpawnPoints[luckSpawnPointIndex]);
                    Instantiate(items[itemIndex], rightSpawnPoints[luckSpawnPointIndex]);
                }
            }
        }
        else
        {
            int leftSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            int rightSpawnPointIndex = Random.Range(0, rightSpawnPoints.Length);

            int leftItemIndex = Random.Range(0, items.Length);
            int rightItemIndex = Random.Range(0, items.Length);

            Instantiate(items[leftItemIndex], leftSpawnPoints[leftSpawnPointIndex]);
            Instantiate(items[rightItemIndex], rightSpawnPoints[rightSpawnPointIndex]);

            int luck = Random.Range(0, luckRate);
            if (luck == 0)
            {
                int luckLeftSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
                int luckRightSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);

                if(luckLeftSpawnPointIndex != leftSpawnPointIndex)
                {
                    Instantiate(items[leftItemIndex], leftSpawnPoints[luckLeftSpawnPointIndex]);
                }
                if(luckRightSpawnPointIndex != rightSpawnPointIndex)
                {
                    Instantiate(items[rightItemIndex], rightSpawnPoints[luckRightSpawnPointIndex]);
                }
            }
        }
    }

    void SpawnItemMerged()
    {
        int spawnPointIndex = Random.Range(0, mergedSpawnPoints.Length);
        int itemIndex = Random.Range(0, items.Length);

        Instantiate(items[itemIndex], mergedSpawnPoints[spawnPointIndex]);

        int luck = Random.Range(0, luckRate);
        if (luck == 0)
        {
            int luckSpawnPointIndex = Random.Range(0, leftSpawnPoints.Length);
            if (luckSpawnPointIndex != spawnPointIndex)
            {
                Instantiate(items[itemIndex], mergedSpawnPoints[luckSpawnPointIndex]);
            }
        }
    }
}
