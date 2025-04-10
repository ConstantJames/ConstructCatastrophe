using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEvent : MonoBehaviour
{
    public GameObject windPrefab1;
    //public GameObject windPrefab2;
    public GameObject windSpawn1;
   // public GameObject windSpawn2;
    public float FanDuration = 1.0f;
    public float SpawnRate = 0.3f;

    public GameManager gameManager;

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    public void StartWind()
    {
        StartCoroutine(WindBlow());
    }

    //vvv Manually trigger windPrefab spawn vvv
    public void Update()
    {
        if(gameManager != null && gameManager.developerMode)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(WindBlow());
            }
        }        
    }

    private IEnumerator WindBlow()
    {
        float elapsedTime = 0f;

        while (elapsedTime<FanDuration)
        {
            elapsedTime += SpawnRate;
            Instantiate(windPrefab1, windSpawn1.transform.position, windPrefab1.transform.rotation);
            //Instantiate(windPrefab2, windSpawn2.transform.position, windPrefab2.transform.rotation);
            yield return new WaitForSeconds(SpawnRate);
        }
        
        yield return null;
    }
}
