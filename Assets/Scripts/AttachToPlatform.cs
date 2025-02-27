using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    /* Will use this script to automatically set anything inside the Build Area (most likely through a trigger) 
     * as a child of the platform/shaker table, so they get affected by rotations/movements that happen to the table. */

    void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform); // can set this under an if statement with CompareTag
    }
}
