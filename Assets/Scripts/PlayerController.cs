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

    public bool canMove = true;
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

    private bool dropIsHeld;
    public float dropTimer = 0.0f;

    private Transform playerHands;
    private GameObject objectInHands;
    private Rigidbody rbObject;
    private Collider colObject;
    private Renderer rendObject;
    private BoxCollider colChild;
    private List<GameObject> colorChanged = new List<GameObject>();

    private bool pickupButton;
    private bool dropReleased;
    private bool hasObject = false;

    public bool massPowerup = false;
    public bool jumpPowerup = false;
    public GameObject playerHat;
    public GameObject playerBoot;
    public Material metalMat;

    public RotatePlatform rotatePlatformLeft;
    public RotatePlatform rotatePlatformRight;
    public Points points;

    public GameObject spawnPointOne;
    public GameObject spawnPointTwo;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        playerHands = gameObject.transform.GetChild(0);

        // Set every object that can be picked up in "Pickable" and every enemy in "Enemy"
        pickableLayerMask = LayerMask.GetMask("Pickable") | LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player");
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
                dropIsHeld = Input.GetKey(KeyCode.K);
                dropReleased = Input.GetKeyUp(KeyCode.K);
                break;
            case Player.Two:
                horizontalVelocity = Input.GetAxis("Horizontal2");
                verticalVelocity = Input.GetAxis("Vertical2");
                spaceDown = Input.GetButtonDown("Jump2");
                pickupButton = Input.GetKeyDown(KeyCode.Keypad1);
                dropIsHeld = Input.GetKey(KeyCode.Keypad2);
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

        if (!canMove && GroundCheck())
        {
            canMove = true;
        }

        // Object detection + pick up and drop object
        Vector3 fwd = transform.TransformDirection(Vector3.forward) + transform.up * -0.5f;

        RaycastHit hit;
        // Debug.DrawRay(transform.position, fwd * 10, Color.yellow);

        if (!canMove)
        {
            return;
        }

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

                if (objectInHands.layer == 6) // objects
                {
                    rendObject = objectInHands.GetComponent<Renderer>();

                    // Increases mass of object if powerup is active
                    if (massPowerup)
                    {
                        if (colorChanged.Count > 0) { ClearList(); }

                        rbObject.mass = 40;

                        if (!colorChanged.Contains(objectInHands))
                        {
                            Color dull = new Color(rendObject.material.color.r / 2, rendObject.material.color.g / 2, rendObject.material.color.b / 2);
                            rendObject.material.SetColor("_Color", dull);

                            colorChanged.Add(objectInHands);
                        }
                    }

                    // Make objects transparent while in player's hands
                    Material objectMat = rendObject.material;
                    ChangeRenderingMode renderMode = objectInHands.GetComponent<ChangeRenderingMode>();
                    renderMode.TransparentMode(objectMat);

                    Color transparent = new Color(rendObject.material.color.r, rendObject.material.color.g, rendObject.material.color.b, 0.5f);
                    rendObject.material.SetColor("_Color", transparent);
                }
                else if (objectInHands.layer == 7) // enemies
                {
                    NavMeshAgent navEnemy = objectInHands.GetComponent<NavMeshAgent>();
                    navEnemy.enabled = false;

                    if (objectInHands.transform.childCount > 0)
                    {
                        colChild = objectInHands.GetComponentInChildren<BoxCollider>();
                        colChild.enabled = false;
                    }
                }
                else if (objectInHands.layer == 9) // players
                {
                    PlayerController playerController = objectInHands.GetComponent<PlayerController>();
                    playerController.canMove = false;
                }

                if (points.objectsInArea.Contains(objectInHands))
                {
                    points.RemovePoints(objectInHands);
                }

                if (rotatePlatformLeft.objectsOnButton.Contains(objectInHands))
                {
                    rotatePlatformLeft.objectsOnButton.Remove(objectInHands);
                }
                else if (rotatePlatformRight.objectsOnButton.Contains(objectInHands))
                {
                    rotatePlatformRight.objectsOnButton.Remove(objectInHands);
                }

                hasObject = true;
            }
        }

        if (dropIsHeld && hasObject)
        {
            dropTimer += Time.deltaTime;

            if (dropTimer > 2.0f)
            {
                dropTimer = 2.0f;
            }
        }

        if (dropReleased && hasObject)
        {
            rbObject.isKinematic = false;
            colObject.enabled = true;

            if (objectInHands.layer == 6)
            {
                Color original = new Color(rendObject.material.color.r, rendObject.material.color.g, rendObject.material.color.b, 1.0f);
                rendObject.material.SetColor("_Color", original);

                Material objectMat = rendObject.material;
                ChangeRenderingMode renderMode = objectInHands.GetComponent<ChangeRenderingMode>();
                renderMode.OpaqueMode(objectMat);
            }
            else if ((objectInHands.layer == 7) && (objectInHands.transform.childCount > 0))
            {
                colChild.enabled = true;
            }

            // Checks how long button is held down for, throws if held for 0.5 seconds
            if (dropTimer > 0.5f)
            {
                Vector3 throwVelocity = (transform.forward * dropTimer * 9.0f) + (transform.up * dropTimer * 5.0f);
                rbObject.velocity = throwVelocity;

                objectInHands.transform.parent = null;
            }
            else
            {
                objectInHands.transform.parent = null;
            }

            dropTimer = 0.0f;
            hasObject = false;
        }
    }

    void FixedUpdate()
    {
        if (!canMove || rbPlayer.isKinematic)
        {
            return;
        }

        rbPlayer.velocity = new Vector3(direction.x * speed, rbPlayer.velocity.y, direction.z * speed);

        // Player faces the direction they are moving in (smooth turning)
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

        // Jumping
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

    private void ClearList()
    {
        for (int i = 0; i < colorChanged.Count; i++)
        {
            if (colorChanged[i] == null)
            {
                colorChanged.RemoveAt(i);
                i--;
            }
            else
            {
                continue;
            }
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
        Renderer hatRend = playerHat.GetComponent<Renderer>();
        Material hatOriginal = hatRend.material;
        hatRend.material = metalMat;

        yield return new WaitForSeconds(30);

        hatRend.material = hatOriginal;
        massPowerup = false;
        Debug.Log("Mass Powerup deactivated");
    }

    IEnumerator JumpPowerUpCountdown()
    {
        Debug.Log("Jump Powerup activated - 30 seconds");
        playerBoot.SetActive(true);

        yield return new WaitForSeconds(30);

        playerBoot.SetActive(false);
        jumpPowerup = false;
        canDoubleJump = false;
        Debug.Log("Jump Powerup deactivated");
    }
}
