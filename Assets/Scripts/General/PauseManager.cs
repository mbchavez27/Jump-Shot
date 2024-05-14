using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        if (Time.timeScale == 0 && !GameOverManager.gamovered)
        {
            pauseMenu.SetActive(true);
        }
        else if (Time.timeScale == 1 && !GameOverManager.gamovered)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
