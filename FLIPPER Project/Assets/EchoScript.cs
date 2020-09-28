using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoScript : MonoBehaviour
{
    public bool canEcho;

    public GameObject echoGO;

    private float timeBtwSpawn = 0f;
    [SerializeField] private float startTimeBtwSpawn = 0.1f;

    private void Update()
    {
        if (canEcho)
        {
            if (timeBtwSpawn <= 0)
            {
                //spawn echo game object
                GameObject echoInstance = Instantiate(echoGO, transform.position, Quaternion.identity);
                Destroy(echoInstance, 1f);
                timeBtwSpawn = startTimeBtwSpawn;
            }
            else
            {
                timeBtwSpawn -= Time.deltaTime;
            }
        }
        
    }
}
