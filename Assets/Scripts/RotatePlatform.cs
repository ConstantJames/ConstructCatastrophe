using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RotatePlatform : MonoBehaviour
{
    // Create empty object to set as a parent of the platform. Scale (1, 1, 1) and assign to platform variable
    public Transform platform;

    public float rotationSpeed = 15.0f;
    public List<GameObject> objectsOnButton = new List<GameObject>();

    public enum RotationDirection
    {
        Left = -1,
        Right = 1
    }
    public RotationDirection direction;

    void Update()
    {
        if (objectsOnButton.Count > 0)
        {
            platform.transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime * (int)direction));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Pickable"))
        {
            if (other.transform.root == other.transform)
            {
                objectsOnButton.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy") || other.CompareTag("Pickable"))
        {
            objectsOnButton.Remove(other.gameObject);
        }
    }
}
