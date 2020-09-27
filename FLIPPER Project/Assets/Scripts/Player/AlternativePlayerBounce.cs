using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativePlayerBounce : MonoBehaviour
{
    [Header("Bounce Values")]

    [SerializeField] float bounceStrength = 1;

    [Header("Stun Effect")]

    [SerializeField] private float stunDuration = 0.2f;

    [Header("Dev Requierement")]

    [SerializeField] Rigidbody2D carRgb = null;
    private PlayerMovementScript playerMovementScript;

    //Layer infos:
    LayerMask obstaclesMask;
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
        playerMovementScript = GetComponent<PlayerMovementScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == playerMask)
        {
            Debug.Log("player collision");
            carRgb.AddForce((gameObject.transform.position - collision.gameObject.transform.position).normalized * bounceStrength);

            StartCoroutine(hitStun());
        }
    }

    /// <summary>
    /// Effet appliquer quand les joueurs entre en collision
    /// </summary>
    /// <returns></returns>
    private IEnumerator hitStun()
    {
        playerMovementScript.canMoveHorizontal = false;
        playerMovementScript.canTurbo = false;
        playerMovementScript.canBreak = false;
        playerMovementScript.canStraff = false;

        yield return new WaitForSecondsRealtime(stunDuration);

        playerMovementScript.canMoveHorizontal = true;
        playerMovementScript.canTurbo = true;
        playerMovementScript.canBreak = true;
        playerMovementScript.canStraff = true;
    }

}
