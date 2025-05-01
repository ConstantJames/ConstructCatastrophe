using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Target { Players, Buttons };
    public Target targetSelect;

    private NavMeshAgent agent;
    private Rigidbody rbEnemy;
    private Collider colEnemy;

    private GameObject target = null;
    private GameObject[] players;
    private GameObject[] buttons;
    private int currentButton = 0;
    private float moveCooldown = 7.5f;
    private bool beginLoop = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rbEnemy = GetComponent<Rigidbody>();
        colEnemy = GetComponent<Collider>();

        agent.enabled = false;
        rbEnemy.isKinematic = false;
        transform.SetParent(null);
    }

    void Start()
    {
        switch (targetSelect)
        {
            case Target.Players:
                players = GameObject.FindGameObjectsWithTag("Player");
                target = players[0];
                break;
            case Target.Buttons:
                buttons = GameObject.FindGameObjectsWithTag("Rotate");
                target = buttons[currentButton];
                break;
        }
    }

    void Update()
    {
        switch (targetSelect)
        {
            case Target.Players:
                TargetClosestPlayer();
                break;
            case Target.Buttons:
                if (beginLoop) { TargetButtons(); }
                break;
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
        if (!agent.enabled && colEnemy.enabled && (collision.gameObject.layer == 8 || collision.gameObject.layer == 6))
        {
            agent.enabled = true;
            rbEnemy.isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RotatePlatform>() != null)
        {
            beginLoop = true;
        }
    }

    void TargetClosestPlayer()
    {
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

    void TargetButtons()
    {
        if (moveCooldown > 0.0f)
        {
            moveCooldown -= Time.deltaTime;
            return;
        }

        currentButton++;

        if (currentButton > 1)
        {
            currentButton = 0;
        }

        target = buttons[currentButton];

        moveCooldown = 7.5f;
    }
}
