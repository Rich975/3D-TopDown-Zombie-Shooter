using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    /* Behaviors needed:
    - Wandering about 
    - When triggered by player, they chase him
    */

    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
    [SerializeField] private float chaseCooldown;

    private Transform player;
    private NavMeshAgent agent;
    private bool isTriggered;
    private float cooldownTimer;
    private float wanderTimerCounter;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isTriggered = false;
        cooldownTimer = chaseCooldown;
        wanderTimerCounter = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (isTriggered)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer > 0)
            {
                FollowPlayer();
            }
            else
            {
                isTriggered = false;
                cooldownTimer = chaseCooldown;
            }
        }
        else
        {
            Wander();
        }
    }

    private void FollowPlayer()
    {
        agent.SetDestination(player.position);
        Vector3 direction = player.transform.position - agent.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(direction);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
            isTriggered = false;

    }


    private void Wander()
    {
        wanderTimerCounter -= Time.deltaTime;
        if (wanderTimerCounter <= 0)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTimerCounter = wanderTimer;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}

