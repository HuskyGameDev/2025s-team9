using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public int health = 10;
    public GameController gameController;

    
    public void TakeDamage() {
        health --;

        if (health <= 0) {
            gameController.GameOver();
        }
    }
    
}
