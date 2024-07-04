using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
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
    private Vector3 lastPlayerPosition;

    public float maxHealth = 50f;
    private HealthSystem healthSystem;

    public enum EnemyStates
    { Wandering, Following, Attacking }

    public EnemyStates enemyState;

    private void Start()
    {
        healthSystem = new HealthSystem(maxHealth);

        healthSystem.OnHealthChanged += HandleHealthChanged;
        healthSystem.OnDeath += HandleDeath;

        zombieRenderer = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        isTriggered = false;
        cooldownTimer = chaseCooldown;
        wanderTimerCounter = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
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
                ChangeMyMaterial(wanderMaterial);
            }
        }
        else
        {
            Wander();
        }
    }

    private void FixedUpdate()
    {
        // Check if the enemy can attack the player
        if (isTriggered && Vector3.Distance(transform.position, player.position) <= distanceToAttackPlayer)
        {
            AttackPlayer();
        }
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }

    private void HandleHealthChanged(float healthPercent)
    {
        // Update enemy health bar or other UI if necessary
    }


    private void ChangeMyMaterial(Material material)
    {
        if (zombieRenderer.material != material)
        {
            zombieRenderer.material = material;
        }
    }

    private void HandleDeath()
    {
        // Handle enemy death (e.g., drop loot, play animation)
        Debug.Log("Enemy is dead!");
        Destroy(gameObject);
    }

    private void FollowPlayer()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) > 0.1f) // Avoid stuttering by only updating if position has changed significantly
        {
            agent.SetDestination(player.position);
            lastPlayerPosition = player.position;
        }
        ChangeMyMaterial(attackMaterial);
    }

    private void AttackPlayer()
    {
        //Debug.Log("Attacking player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            cooldownTimer = chaseCooldown; // Reset cooldown when triggered
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