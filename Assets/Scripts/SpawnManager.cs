using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject currentObject;
    public Transform spawnPoint;

    void Start()
    {
        InvokeRepeating("SpawnObject", 1.0f, 3.0f);
    }

    void SpawnObject()
    {
        int randomObject = Random.Range(0, objects.Length);

        currentObject = objects[randomObject];

        Instantiate(currentObject, spawnPoint.position, spawnPoint.rotation);
    }
}
