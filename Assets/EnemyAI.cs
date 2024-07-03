using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    /* Behaviors needed:
    - Wandering about
    - When triggered by player, they chase him
    */

    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;
    [SerializeField] private float chaseCooldown;
    [SerializeField] private float distanceToAttackPlayer = 1.5f;

    [SerializeField] private Material wanderMaterial;
    [SerializeField] private Material attackMaterial;
    private Renderer zombieRenderer;

    private Transform player;
    private NavMeshAgent agent;
    private bool isTriggered;
    private float cooldownTimer;
    private float wanderTimerCounter;

    public enum EnemyStates
    { Wandering, Following, Attacking }

    public EnemyStates enemyState;

    private void Start()
    {
        zombieRenderer = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        isTriggered = false;
        cooldownTimer = chaseCooldown;
        wanderTimerCounter = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (isTriggered)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer > 0)
            {
                FollowPlayer();
                AttackPlayer();
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

        if (isTriggered && Vector3.Distance(transform.position, player.position) <= distanceToAttackPlayer)
        {
            AttackPlayer();
        }
    }


    private void ChangeMyMaterial(Material material)
    {
        zombieRenderer.material = material;  
    }



    private void FollowPlayer()
    {
        agent.SetDestination(player.position);
        //agent.stoppingDistance = 1.5f;

        //turn forward vector towards player
        Vector3 direction = player.transform.position - agent.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(direction);

    }

    private void AttackPlayer()
    {
        Debug.Log("Attacking player");
        ChangeMyMaterial(attackMaterial);


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
        ChangeMyMaterial(wanderMaterial);
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