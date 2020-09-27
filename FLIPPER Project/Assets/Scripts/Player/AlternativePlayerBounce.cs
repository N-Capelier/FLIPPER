using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativePlayerBounce : MonoBehaviour
{
    [Header("Bounce Values")]

    [SerializeField] float bounceStrength = 1;

    [Header("Dev Requierement")]

    [SerializeField] Rigidbody2D carRgb = null;

    //Layer infos:
    LayerMask obstaclesMask;
    LayerMask playerMask;

    private void Start()
    {
        playerMask = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == playerMask)
        {
            Debug.Log("player collision");
            carRgb.AddForce((gameObject.transform.position - collision.gameObject.transform.position).normalized * bounceStrength);
        }
    }


}
