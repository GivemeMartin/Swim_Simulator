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
    private float moveSpeed = 5.0f;
    [SerializeField]
    private float fastMoveSpeed = 8.0f;
    [SerializeField]
    private float turnSpeed = 360.0f;

    [SerializeField]
    private Transform waterPlane;

    [SerializeField] 
    private ParticleSystem bubbles;

    private float currentMoveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMoveSpeed = moveSpeed;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        cachedMoveDirection = new Vector3(horizontalInput, 0, verticalInput);
        cachedMoveDirection.Normalize();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            bubbles.Play();
            currentMoveSpeed = fastMoveSpeed;

        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            bubbles.Stop();
            currentMoveSpeed = moveSpeed;

        }
    }

    private void FixedUpdate()
    {
        rb.velocity = cachedMoveDirection * currentMoveSpeed;
        //waterPlane.position = new Vector3(rb.position.x, 0, rb.position.z);

        if (cachedMoveDirection != Vector3.zero)
        {
            Quaternion q = Quaternion.LookRotation(cachedMoveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.2f);
        }

    }

}
