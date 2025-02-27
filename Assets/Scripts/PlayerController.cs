using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction;
    private LayerMask pickableLayerMask;

    public float speed = 10.0f;
    private float horizontalVelocity;
    private float verticalVelocity;

    public float jumpForce = 10.0f;
    private bool spaceDown = false;
    private bool isJumping = false;
    private bool isGrounded;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        pickableLayerMask = LayerMask.GetMask("Pickable"); // SET EVERY OBJECT THAT CAN BE PICKED UP UNDER "Pickable" LAYER
    }

    void Update()
    {
        // Movement + Jumping
        horizontalVelocity = Input.GetAxis("Horizontal");
        verticalVelocity = Input.GetAxis("Vertical");

        direction = new Vector3(horizontalVelocity, 0, verticalVelocity).normalized;

        spaceDown = Input.GetButtonDown("Jump");

        if (spaceDown)
        {
            isJumping = true;
        }

        // Player faces the direction they are moving in
        Vector3 lookDirection = direction + gameObject.transform.position;
        gameObject.transform.LookAt(lookDirection);
    }

    void FixedUpdate()
    {
        // Movement + Jumping
        rbPlayer.velocity = new Vector3(direction.x * speed, rbPlayer.velocity.y, direction.z * speed);

        if (isJumping && isGrounded)
        {
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            isJumping = false;
        }

        // Detects if an object is in front of the player
        Vector3 fwd = transform.TransformDirection(Vector3.forward) + transform.up * -0.5f;

        Debug.DrawRay(transform.position, fwd * 10, Color.yellow); // Visible raycast in Scene view during play

        if (Physics.Raycast(transform.position, fwd, 10, pickableLayerMask))
        {
            Debug.Log("Object detected");

            // Pick up script goes here
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
