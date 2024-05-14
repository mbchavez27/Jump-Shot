using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float bulletSpeed = 50f;
    int spread;
    public static bool ishitBullet = false;
    public GameObject bulletParticle;
    public Vector3 offset;

    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "floors" || col.tag == "lava" || col.tag == "punchOne" || col.tag == "punchTwo")
        {
            Instantiate(bulletParticle, this.gameObject.transform.position + offset, this.gameObject.transform.rotation);
            ishitBullet = true;
            Destroy(this.gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }

}
