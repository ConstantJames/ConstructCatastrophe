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
    private bool wasGrounded = false;
    private bool canDoubleJump;

    private LayerMask groundLayerMask;
    public float sphereSize = 2.0f;
    public float maxDistance = 2.0f;

    private float startTime;
    private float timePressed;

    private Transform playerHands;
    private GameObject objectInHands;
    private Rigidbody rbObject;
    private Collider colObject;
    private Renderer rendObject;
    private BoxCollider colChild;
    public Material metalMat;

    private bool pickupButton;
    private bool dropButton;
    private bool dropReleased;
    private bool hasObject = false;

    public bool massPowerup = false;
    public bool jumpPowerup = false;

    public RotatePlatform rotatePlatformLeft;
    public RotatePlatform rotatePlatformRight;

    public GameObject spawnPointOne;
    public GameObject spawnPointTwo;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        playerHands = gameObject.transform.GetChild(0);

        // Set every object that can be picked up in "Pickable" and every enemy in "Enemy"
        pickableLayerMask = LayerMask.GetMask("Pickable") | LayerMask.GetMask("Enemy");
        groundLayerMask = LayerMask.GetMask("Ground") | LayerMask.GetMask("Pickable");
    }

    void Update()
    {
        // Inputs
        switch (playerSelect)
        {
            case Player.One:
                horizontalVelocity = Input.GetAxis("Horizontal1");
                verticalVelocity = Input.GetAxis("Vertical1");
                spaceDown = Input.GetButtonDown("Jump1");
                pickupButton = Input.GetKeyDown(KeyCode.J);
                dropButton = Input.GetKeyDown(KeyCode.K);
                dropReleased = Input.GetKeyUp(KeyCode.K);
                break;
            case Player.Two:
                horizontalVelocity = Input.GetAxis("Horizontal2");
                verticalVelocity = Input.GetAxis("Vertical2");
                spaceDown = Input.GetButtonDown("Jump2");
                pickupButton = Input.GetKeyDown(KeyCode.Keypad1);
                dropButton = Input.GetKeyDown(KeyCode.Keypad2);
                dropReleased = Input.GetKeyUp(KeyCode.Keypad2);
                break;
        }

        // Movement + Jumping
        direction = new Vector3(horizontalVelocity, 0, verticalVelocity).normalized;

        if (wasGrounded && !GroundCheck() && jumpPowerup)
        {
            canDoubleJump = true;
        }

        if (spaceDown)
        {
            if (GroundCheck() || canDoubleJump)
            {
                isJumping = true;

                if (canDoubleJump)
                {
                    canDoubleJump = false;
                }
            }
        }

        if (GroundCheck() && jumpPowerup)
        {
            canDoubleJump = false;
        }

        wasGrounded = GroundCheck();

        // Player faces the direction they are moving in (smooth turning)
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.15f);
        }

        // Object detection + pick up and drop object
        Vector3 fwd = transform.TransformDirection(Vector3.forward) + transform.up * -0.5f;
        // Debug.DrawRay(transform.position, fwd * 10, Color.yellow);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, 10, pickableLayerMask))
        {
            if (pickupButton && !hasObject)
            {
                objectInHands = hit.transform.gameObject;
                rbObject = objectInHands.GetComponent<Rigidbody>();
                colObject = objectInHands.GetComponent<Collider>();

                objectInHands.transform.position = playerHands.transform.position;
                objectInHands.transform.parent = playerHands.transform;
                rbObject.isKinematic = true;
                colObject.enabled = false;

                if (objectInHands.layer == 6) // for objects
                {
                    rendObject = objectInHands.GetComponent<Renderer>();
                    Color transparent = new Color(rendObject.material.color.r, rendObject.material.color.g, rendObject.material.color.b, 0.5f);
                    rendObject.material.SetColor("_Color", transparent);
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

                if (objectInHands == rotatePlatformLeft.objectOnButton)
                {
                    rotatePlatformLeft.StopRotation();
                }
                else if (objectInHands == rotatePlatformRight.objectOnButton)
                {
                    rotatePlatformRight.StopRotation();
                }

                // Increases mass of object if powerup is active
                if (massPowerup)
                {
                    rbObject.mass = 40;
                    rendObject.material = metalMat;
                }

                hasObject = true;
            }
        }

        if (dropButton && hasObject)
        {
            startTime = Time.time;
        }

        if (dropReleased && hasObject)
        {
            timePressed = Time.time - startTime;

            if (timePressed > 2.0f)
            {
                timePressed = 2.0f;
            }

            rbObject.isKinematic = false;
            colObject.enabled = true;

            if (objectInHands.transform.childCount > 0)
            {
                colChild.enabled = true;
            }

            if (objectInHands.layer == 6)
            {
                Color original = new Color(rendObject.material.color.r, rendObject.material.color.g, rendObject.material.color.b, 1.0f);
                rendObject.material.SetColor("_Color", original);
            }

            if (timePressed > 0.5f)
            {
                Vector3 throwVelocity = (transform.forward * timePressed * 9.0f) + (transform.up * timePressed * 5.0f);
                rbObject.velocity = throwVelocity;

                objectInHands.transform.parent = null;
            }
            else
            {
                objectInHands.transform.parent = null;
            }

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
        }
    }

    private bool GroundCheck()
    {
        if (Physics.SphereCast(transform.position, sphereSize, -transform.up, out RaycastHit hit, maxDistance, groundLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MassPowerup"))
        {
            massPowerup = true;
            StartCoroutine(MassPowerUpCountdown());
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("JumpPowerup"))
        {
            jumpPowerup = true;
            StartCoroutine(JumpPowerUpCountdown());
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("KillZone"))
        {
            if (colObject != null)
            {
                colObject.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Respawning
        if (other.gameObject.CompareTag("KillZone"))
        {
            switch (playerSelect)
            {
                case Player.One:
                    transform.position = spawnPointOne.transform.position;
                    break;
                case Player.Two:
                    transform.position = spawnPointTwo.transform.position;
                    break;
            }

            hasObject = false;
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
        canDoubleJump = false;
        Debug.Log("Jump Powerup deactivated");
    }
}
