using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rbPlayer;
    private Vector3 direction;

    private float speed = 5.0f;

    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float horizontalVelocity = Input.GetAxis("Horizontal");
        float verticalVelocity = Input.GetAxis("Vertical");

        direction = new Vector3(horizontalVelocity, 0, verticalVelocity).normalized;
    }

    void FixedUpdate()
    {
        rbPlayer.velocity = direction * speed;

        // rbPlayer.AddForce(direction * speed, forceMode);
    }
}
