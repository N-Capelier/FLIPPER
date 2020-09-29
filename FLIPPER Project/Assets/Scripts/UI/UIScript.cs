using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Image fuel;
    [SerializeField] private PlayerMovementScript plMoveScript;

    // Start is called before the first frame update

    void Start()
    {
        plMoveScript = GetComponentInParent<PlayerMovementScript>();
    }

    // Update is called once per frame
    void Update()
    {
        fuel.fillAmount = plMoveScript.turboFuel/100f;
    }
}
