using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayArea : MonoBehaviour
{
    private LayerMask pickableLayerMask;
    private Collider[] hitObjects;
    private bool resetButton;

    void Start()
    {
        pickableLayerMask = LayerMask.GetMask("Pickable");
    }

    void FixedUpdate()
    {
        // Checks all objects inside PlayArea game object, scale should be (50, 40, 50) when in the center
        hitObjects = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, pickableLayerMask);
    }

    void Update()
    {
        // Placeholder - will be put into UI later
        resetButton = Input.GetKeyDown(KeyCode.P);

        if (resetButton)
        {
            foreach (Collider hitObject in hitObjects)
            {
                Destroy(hitObject.gameObject);
            }
        }
    }
}
