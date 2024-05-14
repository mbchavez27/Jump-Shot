using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashDamageManager : MonoBehaviour
{

    public float destroyDelay = .5f;

    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (GameObject.FindGameObjectWithTag("splashDamage") != null)
            {
                StartCoroutine(SplashManager());
            }
        }
    }


    IEnumerator SplashManager()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(GameObject.FindGameObjectWithTag("splashDamage"));
    }
}
