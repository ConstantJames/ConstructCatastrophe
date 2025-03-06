using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindEvent : MonoBehaviour
{
    public GameObject Wind;
    public float FanDuration = 1.0f;
    public float SpawnRate = 0.3f;

    private void Start()
    {
        
    }

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
            Instantiate(Wind);
            yield return new WaitForSeconds(SpawnRate);
        }
        
        yield return null;
    }
}
