using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public float EventsDelayTime = 10f;

    public GameObject playerTwo;

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
        Debug.Log("EARTHQUAKE!");
    }

    void EventTwo()
    {
        Debug.Log("Fans triggered!");
    }

    void EventThree()
    {
        Debug.Log("A.I. Engaged!");
    }
}
