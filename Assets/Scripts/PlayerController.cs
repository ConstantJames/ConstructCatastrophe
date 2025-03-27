using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    public enum Player { One, Two };
    public Player playerSelect;
    
    private Rigidbody rbPlayer;
    private LayerMask pickableLayerMask;

    public float speed = 10.0f;
    private Vector3 direction;
    private float horizontalVelocity;
    private float verticalVelocity;

    public float jumpForce = 14.0f;
    private bool spaceDown = false;
    private bool isJumping = false;
    private bool isGrounded;
    private bool doubleJump;

    private Transform playerHands;
    private GameObject objectInHands;
    private Rigidbody rbObject;
    private Collider colObject;
    private Renderer rendObject;
    private BoxCollider colChild; // maybe temp
    private bool pickupButton;
    private bool dropButton;
    private bool hasObject = false;

    public bool massPowerup = false;
    public bool jumpPowerup = false;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        playerHands = gameObject.transform.GetChild(0);
        // Set every object that can be picked up in "Pickable" and every enemy in "Enemy"
        pickableLayerMask = LayerMask.GetMask("Pickable") | LayerMask.GetMask("Enemy");
    }

    void Update()
    { 
        // Inputs
        if (playerSelect == Player.One)
        {
            horizontalVelocity = Input.GetAxis("Horizontal1");
            verticalVelocity = Input.GetAxis("Vertical1");

            spaceDown = Input.GetButtonDown("Jump1");

            pickupButton = Input.GetKeyDown(KeyCode.J);
            dropButton = Input.GetKeyDown(KeyCode.K);
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

        if (spaceDown)
        {
            if (isGrounded || doubleJump)
            {
                isJumping = true;
                isGrounded = false;
            }
        }

        if (isGrounded && !spaceDown)
        {
            doubleJump = false;
        }

        // Player faces the direction they are moving in with smooth turning
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }

        // Object detection + pick up and drop object
        Vector3 fwd = transform.TransformDirection(Vector3.forward) + transform.up * -0.5f;

        Debug.DrawRay(transform.position, fwd * 10, Color.yellow); // Visible raycast in Scene view during play

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 10, pickableLayerMask))
        {   
            if (playerSelect == Player.One)
            {
                Debug.Log("Player " + playerSelect + ": Object detected - Press E to pick up and Q to drop");
            }
            else
            {
                Debug.Log("Player " + playerSelect + ": Object detected - Press Numpad1 to pick up and Numpad2 to drop");
            }

            if (pickupButton && !hasObject)
            {
                objectInHands = hit.transform.gameObject;
                rbObject = objectInHands.GetComponent<Rigidbody>();
                colObject = objectInHands.GetComponent<Collider>();
                rendObject = objectInHands.GetComponent<Renderer>();

                objectInHands.transform.position = playerHands.transform.position;
                objectInHands.transform.parent = playerHands.transform;
                rbObject.isKinematic = true;
                colObject.enabled = false;

                if (objectInHands.layer == 6) // for objects
                {
                    rendObject.material.SetColor("_Color", Color.green);
                }
                else // for enemies
                {
                    NavMeshAgent navEnemy = objectInHands.GetComponent<NavMeshAgent>();
                    navEnemy.enabled = false;

                    if (objectInHands.transform.childCount > 0)
                    {
                        colChild = objectInHands.GetComponentInChildren<BoxCollider>();
                        colChild.enabled = false;
                    }
                }

                // Increases mass of object if powerup is active
                if (massPowerup)
                {
                    rbObject.mass = 40;
                    // Change how the object looks as well
                }

                hasObject = true;
            }
        }

        if (dropButton && hasObject)
        {
            rbObject.isKinematic = false;
            colObject.enabled = true;

            if (objectInHands.transform.childCount > 0)
            {
                colChild.enabled = true;
            }

            if (objectInHands.layer == 6)
            {
                rendObject.material.SetColor("_Color", Color.white); // Might need rework?
            }

            objectInHands.transform.parent = null;

            hasObject = false;
        }
    }

    void FixedUpdate()
    {
        // Movement + Jumping
        rbPlayer.velocity = new Vector3(direction.x * speed, rbPlayer.velocity.y, direction.z * speed);

        if (isJumping)
        {
            rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, 0, rbPlayer.velocity.z);
            rbPlayer.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJumping = false;

            if (jumpPowerup)
            {
                doubleJump = !doubleJump;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // REWORK NEEDED
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;

            if (jumpPowerup)
            {
                doubleJump = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MassPowerup"))
        {
            massPowerup = true;
            StartCoroutine(MassPowerUpCountdown());
            Destroy(other.gameObject); // can rework this if it breaks visuals
        }
        else if (other.gameObject.CompareTag("JumpPowerup"))
        {
            jumpPowerup = true;
            StartCoroutine(JumpPowerUpCountdown());
            Destroy(other.gameObject); // can rework this if it breaks visuals
        }
    }

    IEnumerator MassPowerUpCountdown()
    {
        Debug.Log("Mass Powerup activated - 30 seconds");
        yield return new WaitForSeconds(30);
        massPowerup = false;
        Debug.Log("Mass Powerup deactivated");
    }

    IEnumerator JumpPowerUpCountdown()
    {
        Debug.Log("Jump Powerup activated - 30 seconds");
        yield return new WaitForSeconds(30);
        jumpPowerup = false;
        doubleJump = false;
        Debug.Log("Jump Powerup deactivated");
    }
}
