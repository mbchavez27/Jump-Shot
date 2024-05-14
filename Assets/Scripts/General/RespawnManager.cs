using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject playerOne, playerTwo;
    public Transform[] playerspawners = new Transform[0];
    float respawnrate;
    public float respawndelay = 1f;

    void Start()
    {
        playerOne.transform.position = playerspawners[3].transform.position;
        playerTwo.transform.position = playerspawners[1].transform.position;
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            //Check if player is dead
            if (!playerOne.activeSelf)
            {
                RespawnPlayerOne();
            }
            else if (!playerTwo.activeSelf)
            {
                RespawnPlayerTwo();
            }
        }
    }

    void RespawnPlayerOne()
    {
        int spawnpos = Random.Range(0, 9);
        respawnrate += 1 * Time.deltaTime;
        if(respawnrate >= respawndelay)
        {
            respawnrate = 0;
            playerOne.transform.position = playerspawners[spawnpos].transform.position;
            playerOne.SetActive(true);
        }
    }

    void RespawnPlayerTwo()
    {
        int spawnpos = Random.Range(0, 9);
        respawnrate += 1 * Time.deltaTime;
        if (respawnrate >= respawndelay)
        {
            respawnrate = 0;
            playerTwo.transform.position = playerspawners[spawnpos].transform.position;
            playerTwo.SetActive(true);
        }
    }


}
