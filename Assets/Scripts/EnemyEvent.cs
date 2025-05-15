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

        RandomEnemy();
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

            Enemy enemy = newEnemy.GetComponent<Enemy>();
            enemy.targetSelect = Enemy.Target.Players;

            Transform child = enemy.transform.Find("PushBot1/MainBody");
            Renderer enemyRend = child.gameObject.GetComponent<Renderer>();
            Color enemyColor = new Color(0.0f, 0.0f, 0.65f); // blue
            enemyRend.material.SetColor("_Color", enemyColor);

            enemies.Add(newEnemy);
        }

        if (enemies.Count < 4)
        {
            yield return new WaitForSeconds(1);

            GameObject newEnemy = Instantiate(enemyPrefab, spawnTwo.transform);

            Enemy enemy = newEnemy.GetComponent<Enemy>();
            enemy.targetSelect = Enemy.Target.Buttons;

            Transform child = enemy.transform.Find("PushBot1/MainBody");
            Renderer enemyRend = child.gameObject.GetComponent<Renderer>();
            Color enemyColor = new Color(0.65f, 0.0f, 0.0f); // red
            enemyRend.material.SetColor("_Color", enemyColor);

            enemies.Add(newEnemy);
        }
    }

    void RandomEnemy()
    {
        int fiftyFifty = Random.Range(0, 2);
        
        if (fiftyFifty == 0)
        {

        }
    }
}
