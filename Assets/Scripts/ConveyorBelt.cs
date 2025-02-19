using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float velocity;
    public Vector3 direction;

    void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Vector3 movement = velocity * direction * Time.deltaTime;
            collision.gameObject.GetComponent<Rigidbody>().MovePosition(collision.gameObject.transform.position + movement);
        }
    }
}
