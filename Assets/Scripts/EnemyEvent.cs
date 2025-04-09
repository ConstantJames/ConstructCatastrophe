using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvent : MonoBehaviour
{
    public GameObject spawnOne;
    public GameObject spawnTwo;
    public GameObject enemyPrefab;
    public List<GameObject> enemies = new List<GameObject>();

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
        // Spawns enemies, limits how many can be active at one time (4)
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                continue;
            }
            else
            {
                enemies.RemoveAt(i);
            }
        }

        if (enemies.Count < 4)
        {
            GameObject newEnemyOne = Instantiate(enemyPrefab, spawnOne.transform);
            enemies.Add(newEnemyOne);

            yield return new WaitForSeconds(1);

            GameObject newEnemyTwo = Instantiate(enemyPrefab, spawnTwo.transform);
            enemies.Add(newEnemyTwo);
        }
    }
}
