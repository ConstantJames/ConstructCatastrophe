using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip earthquakeSound;
    [SerializeField]
    private AudioClip windSound;
    [SerializeField]
    private AudioClip robotSound;
    [SerializeField]
    private AudioClip warningAlarm;

    public void PlaySound(string soundType)
    {
        // vvv ENTER NEW SOUND CLIPS BELOW vvv
        switch (soundType)
        {
            case "EarthQuake":
                audioSource.clip = earthquakeSound;
                break;
            case "Wind":
                audioSource.clip = windSound;
                break;
            case "A.I. Invasion":
                audioSource.clip = robotSound;
                break;
            case "WarningAlarm":
                audioSource.clip = warningAlarm;
                break;
        }
        audioSource.Play();
    }
}
