using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowChargeBar : MonoBehaviour
{
    public PlayerController playerOneScript;
    public PlayerController playerTwoScript;

    public GameObject playerOne;
    public GameObject playerTwo;
    public GameObject barObjectOne;
    public GameObject barObjectTwo;

    public Slider throwSliderOne;
    public Slider throwSliderTwo;

    public float offsetVert = 7.5f;
    private RectTransform canvasRect;

    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        UpdatePlayerOne();

        if (!playerTwoScript.enabled)
        {
            return;
        }

        UpdatePlayerTwo();
    }

    void UpdatePlayerOne()
    {
        // Sets slider's position above the player
        float offsetPosY = playerOne.transform.position.y + offsetVert;
        Vector3 offsetPos = new Vector3(playerOne.transform.position.x, offsetPosY, playerOne.transform.position.z);

        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        barObjectOne.transform.localPosition = canvasPos;

        // Shows slider if button is held long enough
        if (playerOneScript.dropTimer > 0.5f)
        {
            barObjectOne.SetActive(true);
        }
        else
        {
            barObjectOne.SetActive(false);
        }

        // Slider Value
        throwSliderOne.value = playerOneScript.dropTimer;
    }

    void UpdatePlayerTwo()
    {
        // Sets slider's position above the player
        float offsetPosY = playerTwo.transform.position.y + offsetVert;
        Vector3 offsetPos = new Vector3(playerTwo.transform.position.x, offsetPosY, playerTwo.transform.position.z);

        Vector2 canvasPos;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(offsetPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);

        barObjectTwo.transform.localPosition = canvasPos;

        // Shows slider if button is held long enough
        if (playerTwoScript.dropTimer > 0.5f)
        {
            barObjectTwo.SetActive(true);
        }
        else
        {
            barObjectTwo.SetActive(false);
        }

        // Slider Value
        throwSliderTwo.value = playerTwoScript.dropTimer;
    }
}
