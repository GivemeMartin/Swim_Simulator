using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class FloatingForce : MonoBehaviour
{
    public bool isFloatable;

    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFloatable)
        {
            if (transform.position.y < WaterManager.Instance.waterHeight)
            {
                rigidbody.AddForce(Vector3.up * WaterManager.Instance.floatForceK,ForceMode.Impulse);
            }
        }
    }
}
