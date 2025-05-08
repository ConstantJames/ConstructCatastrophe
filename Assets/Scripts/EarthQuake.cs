using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthQuake : MonoBehaviour
{
    public float EarthquakeIntensity = 0.5f; // Intensity of the Earthquake
    public float EarthquakeDuration = 1.0f;  // Duration of the Earthquake in seconds
    public float EarthquakeFrequency = 0.1f; // Frequency of the Earthquake updates
    public float RotationIntensity = 0.2f;   // Rotation intensity in degrees
    public GameManager gameManager;
    public GameObject cam;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 camOriginalPosition;
    private Quaternion camOriginalRotation;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        camOriginalPosition = cam.transform.localPosition;
        camOriginalRotation = cam.transform.localRotation;
    }

    public void Update()
    {
        if (gameManager != null && gameManager.developerMode)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Earthquake());
            }
        }
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

            float rotationX = Random.Range(-RotationIntensity, RotationIntensity);
            float rotationY = Random.Range(-RotationIntensity, RotationIntensity);
            float rotationZ = Random.Range(-RotationIntensity, RotationIntensity);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, offsetZ);
            transform.localRotation = originalRotation * Quaternion.Euler(rotationX, rotationY, rotationZ);

            cam.transform.localPosition = camOriginalPosition + new Vector3(offsetX, offsetY, offsetZ);
            cam.transform.localRotation = camOriginalRotation * Quaternion.Euler(rotationX, rotationY, rotationZ);

            yield return new WaitForSeconds(EarthquakeFrequency);
        }

        // Reset the position and rotation after the Earthquake
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        cam.transform.localPosition = camOriginalPosition;
        cam.transform.localRotation = camOriginalRotation;
    }
}
