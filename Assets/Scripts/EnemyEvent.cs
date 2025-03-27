using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    public GameObject spawnOne;
    public GameObject spawnTwo;
    public GameObject enemyPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(Invasion());
        }
    }

    public void StartInvasion()
    {
        StartCoroutine(Invasion());
    }

    private IEnumerator Invasion()
    {
        Instantiate(enemyPrefab, spawnOne.transform);
        yield return new WaitForSeconds(1);
        Instantiate(enemyPrefab, spawnTwo.transform);
    }
}
