using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuButton : MonoBehaviour
{
    public GameObject buttonText = null;
    public float buttonTextScaleValue = 1;
    public Vector3 buttonTextScaleVector = new Vector3(1, 1, 1);
    public bool animate = false;
    public float animationStep = 0.5f;
    public float animationLimMin = 1.5f;
    public float animationLimMax = 0.5f;
    RectTransform buttonTextRT = null;

    // Start is called before the first frame update
    void Start()
    {
        buttonTextRT = buttonText.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animate)
        {
            if()
            SelectedAnimation();
        }
    }

    void SelectedAnimation()
    {
         
    }
}
