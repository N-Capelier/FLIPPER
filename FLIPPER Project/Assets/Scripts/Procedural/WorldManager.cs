using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    [Header("Manager")]
    public MapBuilder mapBuilder = null;
    public ItemBuilder itemBuilder = null;
    public WallBuilder wallBuilder = null;

    [Header("Difficulty")]
    [Range(1f, 2f)]
    public float difficultyMultiplicator = 1f;
    [Range(1f, 30f)]
    public float difficultyUpStartingTime = 10f;

    [HideInInspector] public float difficulty = 1f;
    Clock difficultyTimer = null;

    [Header("Tests")]
    public bool isLevelDesignSymetrical = false;

    private void Awake()
    {
        CreateSingleton();
        difficultyTimer = new Clock(difficultyUpStartingTime);
    }

    private void Update()
    {
        if(difficultyTimer.onFinish)
        {
            print("Difficulty UP !");
            difficulty *= difficultyMultiplicator;
            difficultyTimer.SetTime(difficultyUpStartingTime);
        }
    }
}
