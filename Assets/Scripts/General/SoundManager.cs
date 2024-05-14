using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip playerlavadead,explosion,bullethit;
    public GameObject p1, p2;
    AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            //Enable disable audio
            if (SoundManagerMainMenu.isenabledaudio)
            {
                audioSource.enabled = true;
            }
            else if (!SoundManagerMainMenu.isenabledaudio)
            {
                audioSource.enabled = false;
            }

            //Play death sound
            if (PlayerOneController.isinstadead)
                {
                        PlayerOneController.isinstadead = false;
                       audioSource.PlayOneShot(playerlavadead);
                }
                else if (PlayerTwoController.isinstadead2)
                {
                     PlayerTwoController.isinstadead2 = false;
                     audioSource.PlayOneShot(playerlavadead);
                }

                //Play sound for explosion of rockets
                if (MissileController.getexplosion)
                {
                    MissileController.getexplosion = false;
                    audioSource.PlayOneShot(explosion);
                }

            //Play Sound for bullet hitting walls for anything
            if (bulletController.ishitBullet)
            {
                bulletController.ishitBullet = false;
                audioSource.PlayOneShot(bullethit);
            }

            //volume up
            audioSource.volume = 1f;
        }
        if (Time.timeScale == 0 && !GameOverManager.gamovered)
        {
            audioSource.volume = .2f;
        }
        if (Time.timeScale == 0 && GameOverManager.gamovered)
        {
            audioSource.volume = .1f;
        }
    }

}
