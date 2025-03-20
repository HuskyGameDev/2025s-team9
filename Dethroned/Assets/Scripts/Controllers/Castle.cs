using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;

    public HealthBar healthBar;

    
    void Awake() {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(1);
        }
    }

    void TakeDamage(int damage) {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
