using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private Image healthProgressBar;
    [SerializeField]
    private PlayerMovement playerMovement;

    public int health;
    public int maxHealth = 100;
    public bool isDead = false;
    
    private float healthPercent;

    // Animator
    private int animIDDeath; 
    private Animator animator;


    private void Start()
    {
        isDead = false;
        animator = GetComponent<Animator>();
        animIDDeath = Animator.StringToHash("Death");
        health = maxHealth;
    }

    public void TakeDamage(int amount, Transform hitter)
    {
        health -= amount;
        healthPercent = (float)health / maxHealth;
        healthProgressBar.fillAmount = healthPercent;
        if(health <= 0)
        {
            health = 0;
            isDead = true;
            animator.SetTrigger(animIDDeath);
        }
    }
}
