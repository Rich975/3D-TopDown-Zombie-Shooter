using System;

public class HealthSystem
{
    public event Action<float> OnHealthChanged; // Event to notify health changes

    public event Action OnDeath; // Event to notify when health reaches zero

    private float maxHealth;
    private float currentHealth;

    public HealthSystem(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth == 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        OnHealthChanged?.Invoke(currentHealth / maxHealth);
    }

    public float GetHealth() => currentHealth;

    public float GetMaxHealth() => maxHealth;
}