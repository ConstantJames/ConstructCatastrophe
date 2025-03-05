using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Player { One, Two };
    public Player playerSelect;
    public GameObject playerTwo;
    
    private Rigidbody rbPlayer;
    private LayerMask pickableLayerMask;

    public float speed = 10.0f;
    private Vector3 direction;
    private float horizontalVelocity;
    private float verticalVelocity;

    public float jumpForce = 10.0f;
    private bool spaceDown = false;
    private bool isJumping = false;
    private bool isGrounded;

    private Transform playerHands;
    private GameObject objectInHands;
    private Rigidbody rbObject;
    private Collider colObject;
    private bool pickupButton;
    private bool dropButton;
    private bool hasObject = false;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        playerHands = gameObject.transform.GetChild(0);
        pickableLayerMask = LayerMask.GetMask("Pickable"); // SET EVERY OBJECT THAT CAN BE PICKED UP UNDER "Pickable" LAYER
    }

    void Update()
    {
        // Activate multiplayer - EXTREMELY basic implementation just for proof of concept, will revise later + probably move to another script
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerTwo.SetActive(true);
        }

        // Inputs
        if (playerSelect == Player.One)
        {
            horizontalVelocity = Input.GetAxis("Horizontal1");
            verticalVelocity = Input.GetAxis("Vertical1");

            spaceDown = Input.GetButtonDown("Jump1");

            pickupButton = Input.GetKeyDown(KeyCode.E);
            dropButton = Input.GetKeyDown(KeyCode.Q);
        }
        else if (playerSelect == Player.Two)
        {
            horizontalVelocity = Input.GetAxis("Horizontal2");
            verticalVelocity = Input.GetAxis("Vertical2");

            spaceDown = Input.GetButtonDown("Jump2");

            pickupButton = Input.GetKeyDown(KeyCode.Keypad1);
            dropButton = Input.GetKeyDown(KeyCode.Keypad2);
        }

        // Movement + Jumping
        direction = new Vector3(horizontalVelocity, 0, verticalVelocity).normalized;

        if (spaceDown && isGrounded)
        {
            isJumping = true;
        }

        // Player faces the direction they are moving in
        Vector3 lookDirection = direction + gameObject.transform.position;
        gameObject.transform.LookAt(lookDirection);

        // Object detection + pick up and drop object
        Vector3 fwd = transform.TransformDirection(Vector3.forward) + transform.up * -0.5f;

        Debug.DrawRay(transform.position, fwd * 10, Color.yellow); // Visible raycast in Scene view during play

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 10, pickableLayerMask))
        {
            Debug.Log("Player " + playerSelect + ": Object detected - Press E/Numpad1 to pick up and Q/Numpad2 to drop");

            if (pickupButton && !hasObject)
            {
                objectInHands = hit.transform.gameObject;
                rbObject = objectInHands.GetComponent<Rigidbody>();
                colObject = objectInHands.GetComponent<Collider>();

                objectInHands.transform.position = playerHands.transform.position;
                objectInHands.transform.parent = playerHands.transform;
                rbObject.isKinematic = true;
                colObject.enabled = false;

                hasObject = true;
            }
        }

        if (dropButton && hasObject)
        {
            rbObject.isKinematic = false;
            colObject.enabled = true;

            objectInHands.transform.parent = null;

            hasObject = false;
        }
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
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
