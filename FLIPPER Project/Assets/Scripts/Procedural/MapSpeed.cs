using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpeed : MonoBehaviour
{
    [SerializeField] Animator mapAnimator = null;

    void Update()
    {
        mapAnimator.speed = WorldManager.Instance.difficulty;
    }
}
