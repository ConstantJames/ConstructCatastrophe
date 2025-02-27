using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public float minDelay= 10f;
    public float maxDelay = 30f;

    public static EventsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Action> eventsList = new List<Action>();

    public void AddEvent(Action newEvent)
    {
        eventsList.Add(newEvent);
    }

    public void StartRandomEventTrigger()
    {
        StartCoroutine(RandomEvent());
    }

    private IEnumerator RandomEvent()
    {
        while (true)
        {
            // Wait for a random time between 10 and 30 seconds
            float waitTime = UnityEngine.Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            // Select a random event from the list
            int randomIndex = UnityEngine.Random.Range(0, eventsList.Count);
            eventsList[randomIndex]?.Invoke();
        }
    }
}
