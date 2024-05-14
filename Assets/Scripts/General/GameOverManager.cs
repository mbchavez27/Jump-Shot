using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameoverMenu;
    public GameObject redwinner, bluewinner;
    public static bool gamovered;

    public int gameoverscore = 5;// what score is need to win

    void Start()
    {
        //visible not all
        gameoverMenu.SetActive(false);
        bluewinner.SetActive(false);
        redwinner.SetActive(false);

        //game over not yet 
        gamovered = false;
    }

    void Update()
    {
        if (UIManager.p1score == gameoverscore) //red wins
        {
            gamovered = true;
            gameoverMenu.SetActive(true);
            redwinner.SetActive(true);
            Time.timeScale = 0;
        }
        else if(UIManager.p2score == gameoverscore) //blue wins
        {
            gamovered = true;
            gameoverMenu.SetActive(true);
            bluewinner.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
