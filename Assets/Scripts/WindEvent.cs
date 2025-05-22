using System.Collections;
using UnityEngine;

public class WindEvent : MonoBehaviour
{
    public GameObject windPrefab1;
    public GameObject windSpawn1;
    public GameObject windParticle;
    public float FanDuration = 1.0f;
    public float SpawnRate = 0.3f;

    public GameManager gameManager;
    public Animator fanAnim;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    public void StartWind()
    {
        StartCoroutine(WindBlow());
    }

    public void Update()
    {
        if (gameManager != null && gameManager.developerMode)
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
        
        GameObject eventManager = GameObject.Find("EventManager");
        if (eventManager != null)
        {
            SFXManager sfxManager = eventManager.GetComponent<SFXManager>();
            if (sfxManager != null)
            {
                sfxManager.PlaySound("Wind");
            }
            else
            {
                Debug.LogWarning("SFXManager component is missing on EventManager!");
            }
        }
        else
        {
            Debug.LogWarning("EventManager GameObject not found!");
        }

        while (elapsedTime < FanDuration)
        {
            elapsedTime += SpawnRate;
            fanAnim.SetBool("FanAnimPlay", true);
            yield return new WaitForSeconds(2);

            Instantiate(windParticle, windSpawn1.transform.position, windParticle.transform.rotation);
            Instantiate(windPrefab1, windSpawn1.transform.position, windPrefab1.transform.rotation);

            yield return new WaitForSeconds(SpawnRate);
        }

        fanAnim.SetBool("FanAnimPlay", false);
        yield return null;
    }
}