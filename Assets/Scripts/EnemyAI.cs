using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float chaseCooldown = 10f;
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

    public enum EnemyStates { Wandering, Following, Attacking }
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
        switch (enemyState)
        {
            case EnemyStates.Wandering:
                Wander();
                break;
            case EnemyStates.Following:
                FollowPlayer();
                break;
            case EnemyStates.Attacking:
                AttackPlayer();
                break;
        }





        if (isTriggered)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                isTriggered = false;
                cooldownTimer = chaseCooldown;
                ChangeMyMaterial(wanderMaterial);
                enemyState = EnemyStates.Wandering;
            }
        }

    }

    private void FixedUpdate()
    {
        if (isTriggered && Vector3.Distance(transform.position, player.position) <= distanceToAttackPlayer)
        {
            enemyState = EnemyStates.Attacking;
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

    private void HandleDeath()
    {
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
        // Handle attack logic here
        Debug.Log("Attacking player");

        // You can implement the damage to the player here
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            cooldownTimer = chaseCooldown; // Reset cooldown when triggered
            enemyState = EnemyStates.Following;
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

    private void ChangeMyMaterial(Material material)
    {
        if (zombieRenderer.material != material)
        {
            zombieRenderer.material = material;
        }
    }
}
