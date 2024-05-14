using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float bulletSpeed = 60f;
    public GameObject splashdamage;
    public GameObject explosionParticleSystem;
    public GameObject explosionParticleSystem2;
    public static bool getexplosion;
 

    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "bulletOne" || col.tag == "bulletTwo" || col.tag == "floors" || col.tag == "missile" || col.tag == "playerOne" || col.tag == "playerTwo")
        {
            getexplosion = true;
            Instantiate(splashdamage, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Instantiate(explosionParticleSystem, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Instantiate(explosionParticleSystem2, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(this.gameObject);
        }
        else if(col.tag == "lava")
        {
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
