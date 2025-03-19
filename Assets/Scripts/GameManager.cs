using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float EventsDelayTime = 10f;

    public GameObject playerTwo;

    public EarthQuake earthQuake;
    public WindEvent windEvent;    

    private void Start()
    {
        // vvv Add events to the list below vvv
        EventsManager.Instance.AddEvent(EventOne);
        EventsManager.Instance.AddEvent(EventTwo);
        EventsManager.Instance.AddEvent(EventThree);

        // Start the coroutine to delay the event manager
        StartCoroutine(DelayEventManager());
    }

    void Update()
    {
        // Activate multiplayer - EXTREMELY basic implementation just for proof of concept, will revise later
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            playerTwo.SetActive(true);
        }

        // Exit game - Will implement into UI later
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        Debug.Log("A.I. Engaged!");
    }
}
