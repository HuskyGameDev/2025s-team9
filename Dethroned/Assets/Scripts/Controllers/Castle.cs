using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHealth = 20;
    public float currentHealth;

    public HealthBar healthBar;
    public GameController gameController;
    
    void Awake() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update() {
        if(currentHealth <= 0)
        {
            gameController.GameOver();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.SetHealth((int) currentHealth);
    }

}
