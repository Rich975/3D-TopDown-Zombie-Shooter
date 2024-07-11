using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    private Rigidbody rb;
    private float hMovement, vMovement;
    [SerializeField] private float playerSpeed = 30f;

    private Camera mainCamera;
    private Animator animator;

    private bool isFiring;


    public float maxHealth = 100f;
    private HealthSystem healthSystem;
    private HealthBar healthBar;

    [SerializeField] private GameObject damagePanel;

    // Start is called before the first frame update
    private void Start()
    {
        healthSystem = new HealthSystem(maxHealth);
        healthBar = FindObjectOfType<HealthBar>();

        if (healthBar == null)
        {
            Debug.LogError("HealthBar not found in the scene.");
        }

        healthSystem.OnHealthChanged += HandleHealthChanged;
        healthSystem.OnDeath += HandleDeath;

        mainCamera = Camera.main;
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        MouseAim();
        PlayerAttack();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerMovementAnimations();

    }

    public void TakeDamage(float damage)
    {
        healthSystem.TakeDamage(damage);
        healthBar.DamageFlash();  // Trigger damage flash effect
    }

    private void HandleHealthChanged(float healthPercent)
    {
        Debug.Log($"Health changed: {healthPercent * 100}%");
        healthBar.UpdateHealthBar(healthPercent);
    }

    private void HandleDeath()
    {
        // Handle player death (e.g., show game over screen)
        Debug.Log("Player is dead!");
    }

    private void PlayerMovement()
    {
        hMovement = Input.GetAxisRaw("Horizontal");
        vMovement = Input.GetAxisRaw("Vertical");

        if (rb != null)
        {
            Vector3 movement = new Vector3(hMovement, 0, vMovement).normalized * playerSpeed;
            rb.AddForce(movement);
        }
    }

    private void PlayerMovementAnimations()
    {
        // Check the magnitude of the velocity vector
        if (rb.velocity.magnitude > 0.1f) // Adjust the threshold as needed
        {
            animator.SetFloat("isRunning", 1);
        }
        else
        {
            animator.SetFloat("isRunning", 0);

        }
    }

    private void PlayerAttack()
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            animator.SetTrigger("Punch");
        }

    }

    private void GettingAttackedAnimation()
    {
        animator.SetTrigger("GetHit");
    }

    private void MouseAim()
    {
        // Get the mouse position in screen space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform a raycast to find where the mouse is pointing in the world
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            // Get the hit point and set the y to be the same as the player's y (assuming y is up)
            Vector3 mousePosition = hit.point;
            mousePosition.y = transform.position.y; // Keep the weapon at the player's height

            // Calculate the direction from the player to the mouse position
            Vector3 direction = mousePosition - transform.position;

            // Calculate the rotation to look at the mouse position
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Apply the rotation to the weapon, ensuring it only rotates around the y-axis
            transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            TakeDamage(10);
            GettingAttackedAnimation();
        }
    }
}