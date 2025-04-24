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

        GameObject newObject = Instantiate(currentObject, transform.position, currentObject.transform.rotation);

        if (newObject.layer == 6)
        {
            Renderer objectRenderer = newObject.GetComponent<Renderer>();
            Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            objectRenderer.material.color = randomColor;
        }
    }
}
