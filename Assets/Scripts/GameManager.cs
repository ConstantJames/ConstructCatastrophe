using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool developerMode = false;
    public float EventsDelayTime = 10f;

    private bool multiplayer = false;     

    public GameObject playerTwo;
    public GameObject spawnPoint;    

    public EarthQuake earthQuake;
    public WindEvent windEvent;
    public EnemyEvent enemyEvent;
    public WinTrigger winTrigger;
    public GameObject winUI;
    
    private void Start()
    {
        winTrigger = FindObjectOfType<WinTrigger>();
        
        // vvv Add events to the list below vvv
        EventsManager.Instance.AddEvent(EventOne);
        EventsManager.Instance.AddEvent(EventTwo);
        EventsManager.Instance.AddEvent(EventThree);

        if (developerMode == true)
        {
            StopCoroutine(DelayEventManager());
        }
        else
        {
            // Start the coroutine to delay the event manager
            StartCoroutine(DelayEventManager());
        }        
    }

    void Update()
    {
        // Activate multiplayer - EXTREMELY basic implementation just for proof of concept, will revise later
        if (Input.GetKeyDown(KeyCode.Tab) && !multiplayer)
        {
            playerTwo.transform.position = spawnPoint.transform.position;
            playerTwo.GetComponent<PlayerController>().enabled = true;

            multiplayer = true;
        }

        // Exit game - Will implement into UI later
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(winTrigger.winCon)
        {
            StopGame();
        }
    }   
    
    void StopGame()
    {
        Time.timeScale = 0f; // Game will pause

        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
    }   

    private IEnumerator DelayEventManager()
    {
        // Wait for a specified amount of time 
        yield return new WaitForSeconds(EventsDelayTime);

        // Start the random event trigger
        EventsManager.Instance.StartRandomEventTrigger();
    }    

    void EventOne()
    {
        earthQuake.StartEarthquake();
        Debug.Log("EARTHQUAKE!");
    }

    void EventTwo()
    {
        windEvent.StartWind();
        Debug.Log("Fans triggered!");
    }

    void EventThree()
    {
        enemyEvent.StartInvasion();
        Debug.Log("A.I. Engaged!");
    }
}
