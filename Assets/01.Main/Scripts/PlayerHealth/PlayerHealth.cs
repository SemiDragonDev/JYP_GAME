using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead;
    public HealthBarUI healthBar;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Death");
        GetComponent<PlayerMovement>().enabled = false;
    }
}
