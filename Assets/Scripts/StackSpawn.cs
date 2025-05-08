using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSpawn : MonoBehaviour
{
    public GameObject objectPrefab; 
    public Vector3 startPoint = new Vector3(0, 0, 0); // Fixed start position on instantiation
    public int size = 2; // 3x2x3 Cube
    public float spacing = 1.5f; // Spacing between objects
    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();       
    }

    private void Update()
    {
        if (gameManager != null && gameManager.developerMode)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SpawnGrid();
            }
        }
    }

    void SpawnGrid()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    Vector3 position = startPoint + new Vector3(x * spacing, 0, z * spacing);
                    Instantiate(objectPrefab, position, Quaternion.identity);
                }
            }            
        }
    }
}
