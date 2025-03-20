using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls enemy health, damage, and speed
public class EnemyStats : MonoBehaviour
{
    //the game controller for this scene
    GameController gameController = null;

    //various enemy stats, set these in the editor
    [SerializeField] float enemyMaxHealth = 0;
    [SerializeField] private float enemyCurrentHealth = 0; //Serialized so we can check enemy health in the editor

    [SerializeField] float enemyMinSpeed = 0;
    [SerializeField] float enemyMaxSpeed = 0;

    [SerializeField] float enemyDamage = 0;

    private bool touchingTower = false;
    private Tower touchedTower = null;

    //This gets the ai's pathfinding script, we'll use it to set speed
    private AIPath aiPath = null;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed);

        enemyCurrentHealth = enemyMaxHealth;
    }

    //doing things here to keep updates consistant (not tied to fps) this should run around 50 times per second
    private void FixedUpdate()
    {
        if (touchingTower && touchedTower != null)
        {
            touchedTower.DamageHealth(enemyDamage * Time.fixedDeltaTime);
        }

        //When the enemies run out of health destroy them
        if(enemyCurrentHealth <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //mark that the enemy is touching a tower
        if (collision.collider.gameObject.CompareTag("Target"))
        {
            touchingTower = true;
            touchedTower = collision.collider.gameObject.GetComponent<Tower>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //mark that the enemy is no longer touching a tower
        if (collision.collider.gameObject.CompareTag("Target"))
        {
            touchingTower = false;
            touchedTower = null;
        }
    }

    //called when the enemy dies
    private void OnDestroy()
    {
        //TODO: play death sound

        gameController.EnemyDeath(1, 1);
    }

    //deal damage to the enemy this is called on
    public void DamageEnemy(float damage)
    {
        enemyCurrentHealth -= damage;
    }
}
