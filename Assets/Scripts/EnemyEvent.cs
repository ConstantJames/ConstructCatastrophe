using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    public GameObject spawnOne;
    public GameObject spawnTwo;
    public GameObject enemyPrefab;
    public GameManager gameManager;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.developerMode)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(Invasion());
            }
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
