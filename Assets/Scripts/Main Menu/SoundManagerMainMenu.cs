using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerMainMenu : MonoBehaviour
{
    public Toggle sound;
    AudioSource audioSource;
    public static bool isenabledaudio;

    void Start()
    {
        //Set Source
        audioSource = GetComponent<AudioSource>();

        //Set Sound
        sound.isOn = false;
        isenabledaudio = true;
    }
    
    void Update()
    {
        if (!sound.isOn)
        {
            isenabledaudio = true;
        }
        else if (sound.isOn)
        {
            isenabledaudio = false;
        }

        if (isenabledaudio)
        {
            audioSource.volume = 1;
        }
        else if (!isenabledaudio)
        {
            audioSource.volume = 0;
        }
    }
    

}
