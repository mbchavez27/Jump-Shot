using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDisplaySpawner : MonoBehaviour
{
    public GameObject[] displays = new GameObject[5];
    public Transform[] displayspawnpos = new Transform[4];

    public static int totaldisplays;
    float spawnrate,spawnrate2,specialspawnrate,destroyspecialrate,fuelspawnrate;
    public float spawndelay,specialspawndelay,destroyspecialdelay,fuelspawndelay;
    public static bool spawnedspecial = false;
    public static bool spawnedfuel = false;
    public static bool spawnedweapon;
    public static bool spawnedweapon2;

    AudioSource audioSource;
    public AudioClip spawnobject;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //when reset
        spawnedweapon = true;
        spawnedweapon2 = true;

        //Add pistol as basic weapons
        int display1 = Random.Range(0, 3);
        int display2 = Random.Range(0, 3);
        int spawnpos = Random.Range(0, 2); //get different left spawn pos
        int spawnpos2 = Random.Range(2, 4); //get different right spawn pos

        if (spawnedweapon)
        {
           GameObject leftLayer = Instantiate(displays[display1], displayspawnpos[spawnpos].transform.position, displayspawnpos[spawnpos].transform.rotation);
           leftLayer.layer = 10;
        }
        if (spawnedweapon2)
        {
           GameObject rightLayer = Instantiate(displays[display2], displayspawnpos[spawnpos2].transform.position, displayspawnpos[spawnpos2].transform.rotation);
            rightLayer.layer = 11;
        }
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

            //Spawn Weapon from Left Side
            int spawnpos1 = Random.Range(0, 2);
            int display1 = Random.Range(0, 3);
            if (!spawnedweapon)
            {
                spawnrate += 1 * Time.fixedDeltaTime;
                if (spawnrate >= spawndelay)
                {
                    spawnrate = 0;
                    audioSource.PlayOneShot(spawnobject);
                    GameObject leftLayer = Instantiate(displays[display1], displayspawnpos[spawnpos1].transform.position, displayspawnpos[spawnpos1].transform.rotation);
                    leftLayer.layer = 10;
                    spawnedweapon = true;
                }
            }
            //Spawn Weapon from Right Side
            int spawnpos2 = Random.Range(2, 4);
            int display2 = Random.Range(0, 3);
            if (!spawnedweapon2)
            {
                spawnrate2 += 1 * Time.fixedDeltaTime;
                if (spawnrate2 >= spawndelay)
                {
                    spawnrate2 = 0;
                    audioSource.PlayOneShot(spawnobject);
                    GameObject rightlayer = Instantiate(displays[display2], displayspawnpos[spawnpos2].transform.position, displayspawnpos[spawnpos2].transform.rotation);
                    rightlayer.layer = 11;
                    spawnedweapon2 = true;
                }
            }

            //Spawn Special Weapon
            if (!spawnedspecial)
            {
                specialspawnrate += 1 * Time.fixedDeltaTime;
                if (specialspawnrate >= specialspawndelay)
                {
                    specialspawnrate = 0;
                    audioSource.PlayOneShot(spawnobject);
                    Instantiate(displays[4], displayspawnpos[4].transform.position, displayspawnpos[4].transform.rotation);
                    spawnedspecial = true;
                }
            }
            if (spawnedspecial)
            {
                destroyspecialrate += 1 * Time.fixedDeltaTime;
                if (destroyspecialrate >= destroyspecialdelay)
                {
                    destroyspecialrate = 0;
                    Destroy(GameObject.FindGameObjectWithTag("rocket"));
                    spawnedspecial = false;
                }
            }

            //Spawn Special Spot for fuel
            if (!spawnedfuel)
            {
                fuelspawnrate += 1 * Time.fixedDeltaTime;
                if (fuelspawnrate >= fuelspawndelay)
                {
                    fuelspawnrate = 0;
                    audioSource.PlayOneShot(spawnobject);
                    Instantiate(displays[3], displayspawnpos[5].transform.position, displayspawnpos[5].transform.rotation);
                    spawnedfuel = true;
                }
            }
        }
    }
    
}
