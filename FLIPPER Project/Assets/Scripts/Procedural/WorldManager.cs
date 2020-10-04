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

    [Header("Cars")]

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;
    [SerializeField] Sprite splitSprite1, mergeSprite1, splitSprite2, mergeSprite2;

    SpriteRenderer player1Sprite, player2Sprite;

    [Header("Difficulty")]
    [Range(1f, 2f)]
    public float difficultyMultiplicator = 1f;
    [Range(1f, 30f)]
    public float difficultyUpStartingTime = 10f;

    [Range(0f, 10f)]
    public float difficulty = 1f;
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

    [SerializeField]
    Transform splitPos1, splitPos2, mergePos1, mergePos2;

    Clock phaseTimer;
    [HideInInspector] public bool isMerged = false;

    [Header("Tests")]
    public bool isLevelDesignSymetrical = false;

    private void Awake()
    {
        CreateSingleton();
        difficultyTimer = new Clock(difficultyUpStartingTime);
        phaseTimer = new Clock(splitDuration);
        player1Sprite = player1.GetComponent<SpriteRenderer>();
        player2Sprite = player2.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(difficultyTimer.onFinish)
        {
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

                        if(player1 != null)
                        {
                            player1Sprite.sprite = splitSprite1;
                            player1.transform.position = new Vector3(splitPos1.position.x, player1.transform.position.y, player1.transform.position.z);
                        }
                        if (player2 != null)
                        {
                            player2Sprite.sprite = splitSprite2;
                            player2.transform.position = new Vector3(splitPos2.position.x, player2.transform.position.y, player2.transform.position.z);
                        }

                        phaseTimer.SetTime(splitDuration);
                        isMerged = false;
                    }
                    else
                    {
                        mergeCamera.gameObject.SetActive(true);
                        splitCamera.gameObject.SetActive(false);

                        if (player1 != null)
                        {
                            player1Sprite.sprite = mergeSprite1;
                            player1.transform.position = new Vector3(mergePos1.position.x, player1.transform.position.y, player1.transform.position.z);
                        }
                        if (player2 != null)
                        {
                            player2Sprite.sprite = mergeSprite2;
                            player2.transform.position = new Vector3(mergePos2.position.x, player2.transform.position.y, player2.transform.position.z);
                        }

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