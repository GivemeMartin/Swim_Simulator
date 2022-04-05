using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCatchFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fish"))
        {
            var value = other.GetComponent<Fish>().CalculateTotalValue();
            Debug.Log(value);
            Destroy(other.gameObject);
        }
    }
}

