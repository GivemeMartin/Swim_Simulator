using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private Vector3 cachedMoveDirection;

    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 2.0f;
    [SerializeField]
    private float turnSpeed = 360.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        cachedMoveDirection = new Vector3(horizontalInput, 0, verticalInput);
        cachedMoveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        rb.velocity = rb.velocity = cachedMoveDirection * moveSpeed;

        if (cachedMoveDirection != Vector3.zero)
        {
            Quaternion q = Quaternion.LookRotation(cachedMoveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.2f);
        }

    }

}
