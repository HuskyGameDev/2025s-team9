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
    public float enemyMaxHealth = 0;
    [SerializeField] private float enemyCurrentHealth = 0; //Serialized so we can check enemy health in the editor

    //the "default" enemy speed
    [SerializeField] float enemySpeed = 0;

    //wil generate a random number from the negative to positve value of this number and add it to the "default" speed
    [SerializeField] float enemySpeedVariability = 0;

    [SerializeField] float enemyDamage = 0;

    private bool touchingTower = false;
    private Tower touchedTower = null;

    private bool touchingCastle = false;
    private Castle touchedCastle = null;

    private int wavePoints = 0;
    private int soulPoints = 0;

    //This gets the ai's pathfinding script, we'll use it to set speed
    private AIPath aiPath = null;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = enemySpeed + Random.Range(-enemySpeedVariability, enemySpeedVariability);

        enemyCurrentHealth = enemyMaxHealth;
    }

    //doing things here to keep updates consistant (not tied to fps) this should run around 50 times per second
    private void FixedUpdate()
    {
        //attack towers
        if (touchingTower && touchedTower != null)
        {
            touchedTower.DamageHealth(enemyDamage * Time.fixedDeltaTime);
        }

        //attack castle
        if (touchingCastle && touchedCastle != null)
        {
            touchedCastle.TakeDamage(enemyDamage * Time.fixedDeltaTime);
        }

        //When the enemies run out of health destroy them
        if (enemyCurrentHealth <= 0)
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

        //mark that the enemy is touching the castle
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            touchingCastle = true;
            touchedCastle = collision.collider.gameObject.GetComponent<Castle>();
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

        //mark that the enemy is no longer touching the castle
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            touchingCastle = false;
            touchedCastle = null;
        }
    }

    //called when the enemy dies
    private void OnDestroy()
    {
        //TODO: play death sound

        gameController.EnemyDeath(soulPoints, wavePoints);
    }

    //deal damage to the enemy this is called on
    public void DamageEnemy(float damage)
    {
        enemyCurrentHealth -= damage;
    }

    //set info for returning wave/soul points (call this on enemy instatiation)
    public void SetSpawnInfo(int wavePoints, int soulPoints)
    {
        this.wavePoints = wavePoints;
        this.soulPoints = soulPoints;
    }
}
