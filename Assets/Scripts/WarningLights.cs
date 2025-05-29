using System.Collections;
using UnityEngine;

public class WarningLights : MonoBehaviour
{
    [SerializeField]
    private Light environmentLight;
    private EventsManager eventsManager;
    private Coroutine flashingCoroutine;

    private void Start()
    {
        eventsManager = FindAnyObjectByType<EventsManager>();
        environmentLight = GetComponent<Light>();

        StartCoroutine(WatchForCountdown());
    }

    private IEnumerator WatchForCountdown()
    {
        while (true)
        {
            if (eventsManager.countdownStarted)
            {
                if (flashingCoroutine == null) // Start Flashing when countdown begins
                {
                    flashingCoroutine = StartCoroutine(Flashing());
                }
            }
            else
            {
                if (flashingCoroutine != null) // Stop Flashing when countdown ends
                {
                    StopCoroutine(flashingCoroutine);
                    flashingCoroutine = null;
                    environmentLight.enabled = true; // Light stays ON after countdown ends
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator Flashing()
    {
        GameObject eventManager = GameObject.Find("EventManager");
        if (eventManager != null)
        {
            SFXManager sfxManager = eventManager.GetComponent<SFXManager>();
            if (sfxManager != null)
            {
                sfxManager.PlaySound("WarningAlarm");
            }
        }

        while (eventsManager.countdownStarted)
        {
            yield return new WaitForSeconds(1f);
            environmentLight.enabled = !environmentLight.enabled; 
        }
    }
}