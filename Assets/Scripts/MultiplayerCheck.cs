using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCheck : MonoBehaviour
{
    public bool multiplayer = false;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void EnableMultiplayer()
    {
        multiplayer = true;
    }
}
