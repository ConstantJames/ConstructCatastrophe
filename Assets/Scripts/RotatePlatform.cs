using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    // Create empty object to set as a parent of the platform. Scale (1, 1, 1) and assign to platform variable
    public Transform platform;

    private bool isOnButton = false;
    public float rotationSpeed = 15.0f;
    public GameObject objectOnButton;

    public enum RotationDirection
    {
        Left = -1,
        Right = 1
    }
    public RotationDirection direction;

    void Update()
    {
        if (isOnButton)
        {
            platform.transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime * (int)direction));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Pickable"))
        {
            StartRotation(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Pickable"))
        {
            StopRotation();
        }
    }

    public void StartRotation(GameObject obj)
    {
        isOnButton = true;
        objectOnButton = obj;
    }

    public void StopRotation()
    {
        isOnButton = false;
        objectOnButton = null;
    }
}
