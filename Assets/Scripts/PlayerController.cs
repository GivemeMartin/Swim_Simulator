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
    private ParticleSystem bubbles;

    [SerializeField]
    private ParticleSystem bubblesInteract;

    private float currentMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
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
        rb.velocity = rb.velocity = cachedMoveDirection * currentMoveSpeed;

        if (cachedMoveDirection != Vector3.zero)
        {
            Quaternion q = Quaternion.LookRotation(cachedMoveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, q, 0.2f);
        }

    }

}
