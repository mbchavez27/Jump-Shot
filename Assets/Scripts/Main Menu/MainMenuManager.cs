using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject p2controls, p1controls;

    void Start()
    {
        p2controls.SetActive(false);
        p1controls.SetActive(false);
    }  

    public void ViewControls()
    {
        if(p1controls.activeSelf && p2controls.activeSelf)
        {
            p2controls.SetActive(false);
            p1controls.SetActive(false);
        }
        else if(!p1controls.activeSelf && !p2controls.activeSelf)
        {
            p2controls.SetActive(true);
            p1controls.SetActive(true);
        }
    }
 


    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
