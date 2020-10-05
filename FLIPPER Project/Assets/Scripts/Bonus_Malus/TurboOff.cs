using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboOff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(other.GetComponent<PlayerMovementScript>().TurboOff());
        }
    }
}
