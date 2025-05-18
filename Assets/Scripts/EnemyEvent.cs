using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        int fiftyFifty = Random.Range(0, 2);

        if (enemies.Count < 4)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnOne.transform);
            RandomEnemy(newEnemy, fiftyFifty);

            enemies.Add(newEnemy);
        }

        yield return new WaitForSeconds(1);
        if (fiftyFifty == 0) { fiftyFifty = 1; }
        else { fiftyFifty = 0; }

        if (enemies.Count < 4)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnTwo.transform);
            RandomEnemy(newEnemy, fiftyFifty);

            enemies.Add(newEnemy);
        }
    }

    void RandomEnemy(GameObject newEnemy, int fiftyFifty)
    {
        if (fiftyFifty == 0)
        {
            // Targets Players
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            enemy.targetSelect = Enemy.Target.Players;

            Transform child = newEnemy.transform.Find("PushBot1/MainBody");
            Renderer enemyRend = child.gameObject.GetComponent<Renderer>();
            Color enemyColor = new Color(0.0f, 0.0f, 0.65f); // blue
            enemyRend.material.SetColor("_Color", enemyColor);
        }
        else
        {
            // Targets Buttons
            Enemy enemy = newEnemy.GetComponent<Enemy>();
            enemy.targetSelect = Enemy.Target.Buttons;

            Transform child = newEnemy.transform.Find("PushBot1/MainBody");
            Renderer enemyRend = child.gameObject.GetComponent<Renderer>();
            Color enemyColor = new Color(0.65f, 0.0f, 0.0f); // red
            enemyRend.material.SetColor("_Color", enemyColor);
        }
    }
}
