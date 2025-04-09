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
        if (Input.GetKeyDown(KeyCode.Space)) {
            TakeDamage(1);
        }

        //TODO: gameover when castle dies (for now just going to print a debug and destroy the castle object)
        if(currentHealth <= 0)
        {
            Debug.Log("Castle is destroyed, game over");
            gameController.GameOver();
            //GameController.state = GameController.State.build;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage) {
        currentHealth -= damage;
        healthBar.SetHealth((int) currentHealth);
    }

}
