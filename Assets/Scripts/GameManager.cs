using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { Play, Pause };
    public GameState state;

    public bool developerMode = false;
    public float EventsDelayTime = 10f;

    private bool countdownStarted = false;
    private float timer;
    public TextMeshProUGUI countdownText;

    public MultiplayerCheck multiCheck;

    public GameObject playerTwo;
    public GameObject spawnPoint;    

    public EarthQuake earthQuake;
    public WindEvent windEvent;
    public EnemyEvent enemyEvent;
    public WinTrigger winTrigger;
    public GameObject winUI;

    public GameObject[] pauseMenu;
    
    private void Start()
    {
        multiCheck = FindObjectOfType<MultiplayerCheck>();
        winTrigger = FindObjectOfType<WinTrigger>();

        // Game Started
        state = GameState.Play;

        foreach (GameObject button in pauseMenu)
        {
            button.SetActive(false);
        }

        StartCoroutine(StartCountdown());

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
        // Pause Menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (state)
            {
                case GameState.Play:
                    Time.timeScale = 0.0f;

                    foreach (GameObject button in pauseMenu)
                    {
                        button.SetActive(true);
                    }

                    state = GameState.Pause;
                    break;
                case GameState.Pause:
                    Time.timeScale = 1.0f;

                    foreach (GameObject button in pauseMenu)
                    {
                        button.SetActive(false);
                    }

                    state = GameState.Play;
                    break;
            }
        }

        if (countdownStarted)
        {
            timer -= Time.unscaledDeltaTime;
            int seconds = Mathf.FloorToInt(timer % 60);

            if (seconds <= 0)
            {
                countdownText.text = "GO!";
                Time.timeScale = 1.0f;
            }
            else
            {
                countdownText.text = seconds.ToString();
            }
        }

        if(winTrigger.winCon)
        {
            StopGame();
        }
    }
    
    void StopGame()
    {
        Time.timeScale = 0.0f; // Game will pause

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

    public void BackToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator StartCountdown()
    {
        timer = 5.0f;
        countdownStarted = true;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(4);
        countdownStarted = false;

        yield return new WaitForSecondsRealtime(1);
        countdownText.gameObject.SetActive(false);
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