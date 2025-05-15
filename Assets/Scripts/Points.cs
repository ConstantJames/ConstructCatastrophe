using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Points : MonoBehaviour
{
    public int totalPoints = 0;
    public List<GameObject> objectsInArea = new List<GameObject>();

    public TextMeshProUGUI pointsText;

    void Update()
    {
        pointsText.text = "Points: " + totalPoints;

        if (totalPoints < 0)
        {
            totalPoints = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable") && !objectsInArea.Contains(other.gameObject))
        {
            AddPoints(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable") && objectsInArea.Contains(other.gameObject))
        {
            RemovePoints(other.gameObject);
        }
    }

    public void AddPoints(GameObject obj)
    {
        objectsInArea.Add(obj);
        int pointsToAdd = obj.GetComponent<ObjectPoints>().pointValue;
        totalPoints += pointsToAdd;
    }

    public void RemovePoints(GameObject obj)
    {
        objectsInArea.Remove(obj);
        int pointsToRemove = obj.GetComponent<ObjectPoints>().pointValue;
        totalPoints -= pointsToRemove;
    }
}
