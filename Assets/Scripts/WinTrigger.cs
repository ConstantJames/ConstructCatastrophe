using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    public float winTimer = 0f;
    private Coroutine winCoroutine;
    public bool winCon = false;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = FindAnyObjectByType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager != null && gameManager.developerMode)
        {
            if (Input.GetKeyDown(KeyCode.CapsLock))
            {
                winCon = true;
                Debug.Log("WIN CONDITION MET!!!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {            
            Debug.Log("Pickable In Trigger");
            winCoroutine = StartCoroutine(CheckWinCondition());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {            
            if (winCoroutine != null)
            {
                Debug.Log("Pickable Exited Trigger!!!");
                StopCoroutine(winCoroutine);
            }
        }
    }

    private IEnumerator CheckWinCondition()
    {
        Debug.Log("Win Timer Started");
        winTimer = 0f;

        while (winTimer < 3f)
        {
            winTimer += Time.deltaTime;
            yield return null;
        }
        if (winTimer >= 3f)
        {
            winCon = true;
            Debug.Log("WIN CONDITION MET!!!");
        }
    }
}
