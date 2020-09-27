using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldManager : Singleton<WorldManager>
{
    [Header("Manager")]
    public ItemBuilder itemBuilder = null;
    public WallBuilder wallBuilder = null;
    [SerializeField] Image whiteScreen = null;

    public Camera splitCamera = null;
    public Camera mergeCamera = null;

    [Header("Difficulty")]
    [Range(1f, 2f)]
    public float difficultyMultiplicator = 1f;
    [Range(1f, 30f)]
    public float difficultyUpStartingTime = 10f;

    [HideInInspector] public float difficulty = 1f;
    Clock difficultyTimer = null;

    [Header("Timings")]
    [Range(10f, 60f)]
    [SerializeField] float splitDuration = 30f;
    [Range(10f, 60f)]
    [SerializeField] float mergeDuration = 20f;
    [Range(0f, 5f)]
    [SerializeField] float transitionDuration = 3f;
    bool isOnTransition = false;
    bool alphaUp = true;
    [Range(0f, 10f)]
    [SerializeField] float alphaUpSpeed = 1f;
    bool camChanged = false;

    Clock phaseTimer;
    [HideInInspector] public bool isMerged = false;

    [Header("Tests")]
    public bool isLevelDesignSymetrical = false;

    private void Awake()
    {
        CreateSingleton();
        difficultyTimer = new Clock(difficultyUpStartingTime);
        phaseTimer = new Clock(splitDuration);
    }

    private void Update()
    {
        if(difficultyTimer.onFinish)
        {
            print("Difficulty UP !");
            difficulty *= difficultyMultiplicator;
            difficultyTimer.SetTime(difficultyUpStartingTime);
        }

        if(phaseTimer.onFinish)
        {
            isOnTransition = true;
        }

        if(isOnTransition)
        {
            if(alphaUp && whiteScreen.color.a <= 1)
            {
                whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, whiteScreen.color.a + alphaUpSpeed * Time.smoothDeltaTime);
            }
            else
            {
                alphaUp = false;
                if(!camChanged)
                {
                    if (isMerged)
                    {
                        mergeCamera.gameObject.SetActive(false);
                        splitCamera.gameObject.SetActive(true);
                        phaseTimer.SetTime(splitDuration);
                        isMerged = false;
                    }
                    else
                    {
                        mergeCamera.gameObject.SetActive(true);
                        splitCamera.gameObject.SetActive(false);
                        phaseTimer.SetTime(mergeDuration);
                        isMerged = true;
                    }
                    camChanged = true; ;
                }
                whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, whiteScreen.color.a - alphaUpSpeed * Time.smoothDeltaTime);
                if(whiteScreen.color.a <= 0)
                {
                    whiteScreen.color = new Color(whiteScreen.color.r, whiteScreen.color.g, whiteScreen.color.b, 0);
                    isOnTransition = false;
                    alphaUp = true;
                    camChanged = false;
                }
            }
        }
    }
}