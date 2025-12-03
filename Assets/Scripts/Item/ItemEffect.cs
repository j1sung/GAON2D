using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public float bobspeed = 2f;
    public float bobAmplitude = 0.1f;

    private Vector2 initialPos;
    private float t;

    void Start()
    {
        initialPos = transform.position;
    }

    
    void LateUpdate()
    {
        t += Time.deltaTime;

        // 위아래 움직임
        float bob = Mathf.Sin(t * bobspeed) * bobAmplitude;

        transform.position = initialPos + new Vector2(0f, bob);
    }
}
