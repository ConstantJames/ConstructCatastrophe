using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEvent : MonoBehaviour
{
    public GameObject Wind;
    //public GameObject Wind2;
    public GameObject spawnPoint;
    public float FanDuration = 1.0f;
    public float SpawnRate = 0.3f;
                   
    public void StartWind()
    {
        StartCoroutine(WindBlow());
    }

    //vvv Manually trigger wind spawn vvv
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(WindBlow());
        }
    }

    private IEnumerator WindBlow()
    {
        float elapsedTime = 0f;

        while (elapsedTime<FanDuration)
        {
            elapsedTime += SpawnRate;
            Instantiate(Wind, spawnPoint.transform.position, spawnPoint.transform.rotation);
            //Instantiate(Wind2);
            yield return new WaitForSeconds(SpawnRate);
        }
        
        yield return null;
    }
}
