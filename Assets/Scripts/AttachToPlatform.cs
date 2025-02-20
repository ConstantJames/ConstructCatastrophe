using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform); // can set this under an if statement
    }
}
