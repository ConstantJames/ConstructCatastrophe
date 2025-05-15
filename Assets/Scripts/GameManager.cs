using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public bool developerMode = false;
    public float EventsDelayTime = 10f;

    public MultiplayerCheck multiCheck;

    public GameObject playerTwo;
    public GameObject spawnPoint;    

    public EarthQuake earthQuake;
    public WindEvent windEvent;
    public EnemyEvent enemyEvent;
    public WinTrigger winTrigger;
    public GameObject winUI;
    
    private void Start()
    {
        multiCheck = FindObjectOfType<MultiplayerCheck>();
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

        // Multiplayer
        if (multiCheck == null)
        {
            return;
        }

        if (multiCheck.multiplayer)
        {
            playerTwo.transform.position = spawnPoint.transform.position;
            playerTwo.GetComponent<PlayerController>().enabled = true;
        }
        else
        {
            playerTwo.GetComponent<PlayerController>().enabled = false;
        }
    }

    void Update()
    {
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