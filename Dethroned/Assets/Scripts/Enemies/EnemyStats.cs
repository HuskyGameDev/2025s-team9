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

    [SerializeField] float enemySpeed = 0;

    [SerializeField] float enemyDamage = 0;

    //This gets the ai's pathfinding script, we'll use it to set speed
    private AIPath aiPath = null;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        aiPath = GetComponent<AIPath>();
        aiPath.maxSpeed = enemySpeed;

        enemyCurrentHealth = enemyMaxHealth;
    }

    private void FixedUpdate()
    {
        //When the enemies run out of health destroy them
        if(enemyCurrentHealth <= 0)
            Destroy(gameObject);
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
