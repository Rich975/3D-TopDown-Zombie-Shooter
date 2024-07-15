using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    [SerializeField] private BoxCollider attackCollider; // Reference to the attack collider
    [SerializeField] private float attackDamage = 10f; // Damage value

    // Start is called before the first frame update
    void Start()
    {
        attackCollider.enabled = false; // Disable the attack collider initially
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This function will be called by the animation event
    public void EnableAttackCollider()
    {
        attackCollider.enabled = true;
    }

    // This function will be called by the animation event
    public void DisableAttackCollider()
    {
        attackCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie")) // Assuming the enemy has the tag "Zombie"
        {
            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log("dealing damage to enemy!");
            }
        }
    }
}
