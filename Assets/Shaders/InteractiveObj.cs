using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObj : MonoBehaviour
{
    public Vector3 lastPos;
    
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - lastPos).sqrMagnitude > 0.1f)
        {
            lastPos = transform.position;
            gameObject.layer = 6;
        }
        else
        {
            gameObject.layer = 0;
        }
            
    }
}
