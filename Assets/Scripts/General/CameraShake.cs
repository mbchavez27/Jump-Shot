using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Transform camerastransform;
    public static float shakeduration = 0f;
    public float shakemagnitude = .1f;
    float damingspeed = 1f;
    Vector3 initialposition;

    void Awake()
    {
        camerastransform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        initialposition = transform.localPosition;
    }

    void Update()
    {
        if (shakeduration > 0)
        {
            transform.localPosition = initialposition + Random.insideUnitSphere * shakemagnitude;
            shakeduration -= damingspeed * Time.deltaTime;
        }
        else
        {
            shakeduration = 0;
            initialposition = transform.localPosition;
            transform.position = new Vector3(0, 0, -10);
        }

        if (Time.timeScale == 0)
        {
            shakeduration = 0;
        }
    }

    public static void ShakeShakeCamera()
    {
        shakeduration = .1f;
    }
}
