using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTexture : MonoBehaviour
{
    public float scrollSpeedX;
    public float scrollSpeedY;
    private MeshRenderer meshRenderer;

    public GameManager gameMg;
    public WinTrigger win;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMg.state == GameManager.GameState.Play && !win.winCon)
        {
            meshRenderer.material.mainTextureOffset = new Vector2(Time.time * scrollSpeedX, Time.time * scrollSpeedY);
        }
    }
}
