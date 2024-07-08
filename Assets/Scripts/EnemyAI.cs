using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [SerializeField] private float wanderRadius = 10f;
    [SerializeField] private float wanderTimer = 5f;
    [SerializeField] private float chaseCooldown = 10f;
    [SerializeField] private float attackDistance = 2f;

    [SerializeField] private Material wanderMaterial;
    [SerializeField] private Material attackMaterial;
    private Renderer zombieRenderer;

    private Transform player;
    private NavMeshAgent agent;
    private bool isTriggered;
    private float cooldownTimer;
    private float wanderTimerCounter;

    public float maxHealth = 50f;
    public float currentHealth;
    private HealthSystem healthSystem;




    public enum EnemyStates { Wandering, Following, Attacking }
    public EnemyStates enemyState;

    private Animator anim;

    private void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthSystem.OnHealthChanged += HandleHealthChanged;
        healthSystem.OnDeath += HandleDeath;

        zombieRenderer = GetComponentInChildren<Renderer>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = SetRandomMovementSpeed();

        isTriggered = false;
        cooldownTimer = chaseCooldown;
        wanderTimerCounter = wanderTimer;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;

        ChangeMyMaterial(wanderMaterial);
        enemyState = EnemyStates.Wandering;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyStates.Wandering:
                Wander();
                SetAnimationState("isIdle");
                break;
            case EnemyStates.Following:
                FollowPlayer();
                SetAnimationState("isFollowing");
                break;
            case EnemyStates.Attacking:
                AttackPlayer();
                SetAnimationState("isAttacking");
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
            else
            {
                ChangeMyMaterial(attackMaterial);
            }
        }
    }


    private float SetRandomMovementSpeed()
    {
        float minSpeed = 3f;
        float maxSpeed = 7f;
        float zombieMovementSpeed = Random.Range(minSpeed, maxSpeed);
        return zombieMovementSpeed;
    }


    private void SetAnimationState(string state)
    {
        anim.SetBool("isIdle", state == "isIdle");
        anim.SetBool("isFollowing", state == "isFollowing");
        anim.SetBool("isAttacking", state == "isAttacking");
    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
        currentHealth -= damage;    
        Debug.Log($"Enemy has {currentHealth} hitpoints left");
        
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
        if (Vector3.Distance(transform.position, player.position) > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }

        if (Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            enemyState = EnemyStates.Attacking;
        }


    }

    private void AttackPlayer()
    {
        // Handle attack logic here
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            enemyState = EnemyStates.Following;
        }
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
            enemyState = EnemyStates.Following;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = false;
            cooldownTimer = chaseCooldown; // Reset cooldown when the player exits the trigger
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        TakeDamage(10);
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

