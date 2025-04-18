using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] objects;
    private GameObject currentObject;

    public float spawnRate = 3.0f;

    void Start()
    {
        InvokeRepeating("SpawnObject", 1.0f, spawnRate);
    }

    void SpawnObject()
    {
        int randomObject = Random.Range(0, objects.Length);

        currentObject = objects[randomObject];

        Instantiate(currentObject, transform.position, currentObject.transform.rotation);
    }
}
