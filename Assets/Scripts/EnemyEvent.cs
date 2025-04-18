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

    private void ClearList()
    {
        // Removes destroyed objects from the list of enemies
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
            else
            {
                continue;
            }
        }
    }

    private IEnumerator Invasion()
    {
        // Spawns enemies, limits how many can be active at one time (4)
        ClearList();

        if (enemies.Count < 4)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnOne.transform);
            enemies.Add(newEnemy);
        }

        if (enemies.Count < 4)
        {
            yield return new WaitForSeconds(1);

            GameObject newEnemy = Instantiate(enemyPrefab, spawnTwo.transform);
            enemies.Add(newEnemy);
        }
    }
}
