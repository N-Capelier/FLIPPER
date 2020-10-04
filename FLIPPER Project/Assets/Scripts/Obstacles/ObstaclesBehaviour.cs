using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesBehaviour : MonoBehaviour
{
    [Header("Behaviour Values")]
    [SerializeField] float fallSpeed = -1;
    [SerializeField] float timeBeforeDestruction = 1f;

    [Header("Procedural Management Values")]
    public int generationPound = 1;              //pour Nico -> si tu veux que certain wall pop plus souvent que d'autre

    [Header("Dev Requierement")]
    [SerializeField] Rigidbody2D obstacleRgb = null;

    //Layer infos
    LayerMask worldLimitMask;
    LayerMask playerMask;

    private void Start()
    {
        worldLimitMask = LayerMask.NameToLayer("World Limit");
        playerMask = LayerMask.NameToLayer("Player");

        obstacleRgb.velocity = new Vector2(0, fallSpeed * WorldManager.Instance.difficulty); 
    }

    private void Update()
    {
        if (obstacleRgb.velocity.y != fallSpeed * WorldManager.Instance.difficulty)
        {
            obstacleRgb.velocity = new Vector2(0, fallSpeed * WorldManager.Instance.difficulty);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == worldLimitMask)
        {
            Destroy(gameObject);
        }
        if(collision.gameObject.layer == playerMask)
        {
            StartCoroutine(DelayedDestruction());
        }
    }

    IEnumerator DelayedDestruction()
    {
        yield return new WaitForSeconds(timeBeforeDestruction);
        Destroy(gameObject);
    }
}
