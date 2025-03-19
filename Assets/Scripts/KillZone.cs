using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    public GameObject spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.transform.position;
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
