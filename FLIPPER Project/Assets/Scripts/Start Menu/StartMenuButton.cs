using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartMenuButton : MonoBehaviour
{
    [Header("Requiered Data")]

    public GameObject buttonText = null;
    public EventSystem StartMenuEventSystem = null;
    RectTransform buttonTextRT = null;
    Button thisButtonComp = null;

    [Header("Global Behaviour Management")]

    public bool animate = true;

    bool isReset = true;

    [Header("Scale Animation Values")]
     
    public float animationScaleStep = 0.5f;
    public float animationScaleLimMin = 0.5f;
    public float animationScaleLimMax = 1.5f;

    float buttonTextScaleValue = 1;
    bool grow = true;

    [Header("Rotation Animation Values")]

    public float animationRotateStep = 0.1f;
    public float animationRotateLimMin = -5f;
    public float animationRotateLimMax = 5f;

    float buttonTextRotateValue = 0;

    void Awake()
    {
        buttonTextRT = buttonText.GetComponent<RectTransform>();
        thisButtonComp = GetComponent<Button>();
    }

    void Update()
    {
        if (StartMenuEventSystem.currentSelectedGameObject == gameObject && animate == true)
        {
            if (isReset) isReset = false;           
            SelectedAnimation();
        }
        else if (!isReset)      //Reset Button Text at default value on unSelected
        {
            buttonTextScaleValue = 1f;
            buttonTextRT.localScale = new Vector3(buttonTextScaleValue, buttonTextScaleValue, buttonTextScaleValue);

            buttonTextRotateValue = 0;
            buttonTextRT.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    void SelectedAnimation()
    {
        if (grow)
        {
            if ( buttonTextScaleValue < animationScaleLimMax - 0.1f * animationScaleStep)
            {
                buttonTextScaleValue = Mathf.Lerp(buttonTextScaleValue, animationScaleLimMax, animationScaleStep * Time.deltaTime);
                buttonTextRT.localScale = new Vector3(buttonTextScaleValue, buttonTextScaleValue, buttonTextScaleValue);

                buttonTextRotateValue = Mathf.Lerp(buttonTextRotateValue, animationRotateLimMax, animationRotateStep * Time.deltaTime);
                buttonTextRT.rotation = Quaternion.Euler(0, 0, buttonTextRotateValue);
            }
            else
            {
                grow = false;
            }
        }
        else
        {
            if (buttonTextScaleValue > animationScaleLimMin + 0.1f * animationScaleStep)
            {
                buttonTextScaleValue = Mathf.Lerp(buttonTextScaleValue, animationScaleLimMin, animationScaleStep * Time.deltaTime);
                buttonTextRT.localScale = new Vector3(buttonTextScaleValue, buttonTextScaleValue, buttonTextScaleValue);

                buttonTextRotateValue = Mathf.Lerp(buttonTextRotateValue, animationRotateLimMin, animationRotateStep * Time.deltaTime);
                buttonTextRT.rotation = Quaternion.Euler(0, 0, buttonTextRotateValue);
            }
            else
            {
                grow = true;
            }
        }
    }

    public void SwitchButton(bool activated)
    {
        if (gameObject.activeInHierarchy)
        {
            thisButtonComp.interactable = activated;
            animate = activated;
        }
    }
}
