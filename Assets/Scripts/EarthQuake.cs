using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour
{
    public float EarthquakeIntensity = 0.5f;
    public float EarthquakeDuration = 1.0f; 
    public float EarthquakeFrequency = 0.1f;

    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.localPosition;
    }

    public void StartEarthquake()
    {
        StartCoroutine(Earthquake());
    }

    private IEnumerator Earthquake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < EarthquakeDuration)
        {
            elapsedTime += EarthquakeFrequency;
            float offsetX = Random.Range(-EarthquakeIntensity, EarthquakeIntensity);
            float offsetY = Random.Range(-EarthquakeIntensity, EarthquakeIntensity);
            float offsetZ = Random.Range(-EarthquakeIntensity, EarthquakeIntensity);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);

            yield return new WaitForSeconds(EarthquakeFrequency);
        }

        // Reset the position after the Earthquake
        transform.localPosition = originalPosition;
    }
}
