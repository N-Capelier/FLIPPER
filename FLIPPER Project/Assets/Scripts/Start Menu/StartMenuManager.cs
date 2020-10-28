using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public EventSystem mainMenuEventSystem = null;

    GameObject lastSelectedGameObject = null;

    public StartMenuButton[] allMenuButton = null;

    public float startAnimationTime = 1.5f;

    private void Start()
    {
        if(mainMenuEventSystem != null)
        {
            lastSelectedGameObject = mainMenuEventSystem.firstSelectedGameObject;
        }

        mainMenuEventSystem.gameObject.SetActive(false);

        foreach(StartMenuButton button in allMenuButton)
        {
            button.SwitchButton(false);
        }

        StartCoroutine(ActivateMenu(startAnimationTime));
    }

    private void Update()
    {
        if(mainMenuEventSystem != null)
        {
            if (mainMenuEventSystem.currentSelectedGameObject == null)
            {
                mainMenuEventSystem.SetSelectedGameObject(lastSelectedGameObject);
            }
            else
            {
                lastSelectedGameObject = mainMenuEventSystem.currentSelectedGameObject;
            }
        }      
    }

    IEnumerator ActivateMenu(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        foreach (StartMenuButton button in allMenuButton)
        {
            button.SwitchButton(true);
        }

        mainMenuEventSystem.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {

    }

    public void Credits()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
