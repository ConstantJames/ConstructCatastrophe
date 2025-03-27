using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Rigidbody rbEnemy;
    private Collider colEnemy;

    private GameObject[] players;
    private GameObject target = null;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rbEnemy = GetComponent<Rigidbody>();
        colEnemy = GetComponent<Collider>();
        players = GameObject.FindGameObjectsWithTag("Player");

        target = players[0];

        agent.enabled = false;
        rbEnemy.isKinematic = false;
        transform.SetParent(null);
    }

    void Update()
    {
        // Selects which player to go for - Will target the closest player
        if (agent.enabled)
        {
            float distanceToClosest = Vector3.Distance(target.transform.position, transform.position);

            foreach (GameObject p in players)
            {
                if (p != target)
                {
                    float distance = Vector3.Distance(p.transform.position, transform.position);

                    if (distance < distanceToClosest)
                    {
                        distanceToClosest = distance;
                        target = p;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (agent.enabled)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!agent.enabled && colEnemy.enabled && collision.gameObject.CompareTag("Ground"))
        {
            agent.enabled = true;
            rbEnemy.isKinematic = true;
        }
    }
}
