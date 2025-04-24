using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using Debug = UnityEngine.Debug;

public class EventsManager : MonoBehaviour
{
    public float minDelay = 10f;
    public float maxDelay = 30f;

    public static EventsManager Instance;

    private string eventName;
    public TextMeshProUGUI warningText;

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

        warningText.text = " ";
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
            // Wait for a random time between 10 and 30 seconds + Selects random event from the list
            float waitTime = UnityEngine.Random.Range(minDelay, maxDelay);
            int randomIndex = UnityEngine.Random.Range(0, eventsList.Count);

            if (randomIndex == 0) { eventName = "Earthquake"; }
            else if (randomIndex == 1) { eventName = "Fans"; }
            else if (randomIndex == 2) { eventName = "Robot Invasion"; }

            // Ten second disaster warning
            yield return new WaitForSeconds(waitTime - 10);

            warningText.text = eventName + " in 10 Seconds!";

            yield return new WaitForSeconds(10);

            // Invokes random event
            eventsList[randomIndex]?.Invoke();
            warningText.text = " ";
        }
    }
}
