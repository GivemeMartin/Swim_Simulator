using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{
    public float rippleInterval;
    public float rippleTimer;

    // Update is called once per frame
    void Update()
    {
        rippleTimer += Time.deltaTime;
        if (rippleTimer > rippleInterval)
        {
            rippleTimer = 0f;
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 0;
        }
    }
}
