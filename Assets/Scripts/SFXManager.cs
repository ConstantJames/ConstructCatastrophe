using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip earthquakeSound;
    public AudioClip windSound;
    public AudioClip robotSound;

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
        }
        audioSource.Play();
    }
}
