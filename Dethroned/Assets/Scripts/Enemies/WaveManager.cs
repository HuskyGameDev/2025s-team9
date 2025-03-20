using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //make sure these arrays are the same length and the indices correspond to each other (otherwise it could lead to some array access errors)
    [Header("Enemy Prefabs and Points (please sort by points low to high)")]
    [SerializeField] GameObject[] strongEnemyPrefabs;
    [SerializeField] int[] strongEnemyPoints;

    [SerializeField] GameObject[] mediumEnemyPrefabs;
    [SerializeField] int[] mediumEnemyPoints;

    [SerializeField] GameObject[] weakEnemyPrefabs;
    [SerializeField] int[] weakEnemyPoints;

    [Header("Other Stuff")]
    //WIP: This is just a quick thing to call from a UI so I can test the enemy database/target finder stuff
    [SerializeField] int enemySpawnCount;
    [SerializeField] int spawnerNum; //this is only the initial spawner location

    [SerializeField] EnemyDatabase enemyDatabase;

    private void Start()
    {
        //make sure spawner listing works
        enemyDatabase.updateSpawnerListings(null);
    }

    public void spawnEnemy()
    {
        enemyDatabase.updateSpawnerListings(null);
        enemyDatabase.updateTargetDatabase(null, false);
        enemyDatabase.spawnEnemies(weakEnemyPrefabs[spawnerNum % 3], enemySpawnCount, spawnerNum%3);

        //this way each click will spawn at different spawner
        spawnerNum++;
    }

    //takes in an amount of points and splits them among varying strengths of enemies (note that the percents should be floats in the range of 0->1)
    public void SpawnWave()
    {
        //function parameters
        //int points, float maxPercentStrong, float minPercentStrong, float maxPercentMedium, float minPercentMedium

        //setting these here temporarily so I can test if this works with button press (without setting up UI)
        int points = 20;
        float maxPercentStrong = .2f;
        float minPercentStrong = .1f;
        float maxPercentMedium = .3f;
        float minPercentMedium = .2f;

        //update the database so we have proper targeting
        enemyDatabase.updateTargetDatabase(null, false);

        //distribute points among various enemy types

        //calculate the amount of points towards strong enemies
        int strongPoints = Random.Range((int)(points * minPercentStrong), (int)(points * maxPercentStrong));

        int remainingPoints = strongPoints;

        //have this so we aren't constantly looking through enemies that are too expensive (gets changed in the loop)
        int bestWeCanAfford = strongEnemyPrefabs.Length;

        //as long as we have enough points to spawn the cheapest strong enemy try to find a random one to spawn
        while (remainingPoints >= strongEnemyPoints[0])
        {
            //pick a random strong enemy to look at in the range of cheapest to most expensive we were last able to spawn
            int randomIndex = Random.Range(0, bestWeCanAfford);

            int currentCost;
            if ((currentCost = strongEnemyPoints[randomIndex]) <= remainingPoints) //if we can afford it spawn them
            {
                //subtract the cost of this enemy from our remaining points
                remainingPoints -= currentCost;

                //spawn the enemy
                enemyDatabase.spawnEnemies(strongEnemyPrefabs[randomIndex], 1, spawnerNum % 3);
                spawnerNum++;
            }
            else //if we can't try to find a cheaper enemy (remember arrays should be sorted low to high in terms of cost)
            {
                for(int i = randomIndex; i < strongEnemyPrefabs.Length; i--)
                {
                    if((currentCost = strongEnemyPoints[randomIndex]) <= remainingPoints)
                    {
                        //subtract the cost of this enemy from our remaining points
                        remainingPoints -= currentCost;

                        //spawn the enemy
                        enemyDatabase.spawnEnemies(strongEnemyPrefabs[i], 1, spawnerNum % 3);
                        spawnerNum++;

                        //this is now the MOST we could possibly afford (probably not even this tbh)
                        bestWeCanAfford = i + 1;

                        //we found a enemy cheap enough to spawn so break out of this loop after we spawn them
                        break;
                    }
                }
            }
        }

        //calculate the amount of points towardsmedium strength enemies
        int mediumPoints = Random.Range((int)(points * minPercentMedium), (int)(points * maxPercentMedium));

        remainingPoints += mediumPoints; //carry over points from strong enemies
        bestWeCanAfford = mediumEnemyPrefabs.Length;

        //as long as we have enough points to spawn the cheapest medium enemy try to find a random one to spawn
        while (remainingPoints >= mediumEnemyPoints[0])
        {
            //pick a random medium enemy to look at in the range of cheapest to most expensive we were last able to spawn
            int randomIndex = Random.Range(0, bestWeCanAfford);

            int currentCost;
            if ((currentCost = mediumEnemyPoints[randomIndex]) <= remainingPoints) //if we can afford it spawn them
            {
                //subtract the cost of this enemy from our remaining points
                remainingPoints -= currentCost;

                //spawn the enemy
                enemyDatabase.spawnEnemies(mediumEnemyPrefabs[randomIndex], 1, spawnerNum % 3);
                spawnerNum++;
            }
            else //if we can't try to find a cheaper enemy (remember arrays should be sorted low to high in terms of cost)
            {
                for (int i = randomIndex; i < mediumEnemyPrefabs.Length; i--)
                {
                    if ((currentCost = mediumEnemyPoints[randomIndex]) <= remainingPoints)
                    {
                        //subtract the cost of this enemy from our remaining points
                        remainingPoints -= currentCost;

                        //spawn the enemy
                        enemyDatabase.spawnEnemies(mediumEnemyPrefabs[i], 1, spawnerNum % 3);
                        spawnerNum++;

                        //this is now the MOST we could possibly afford (probably not even this tbh)
                        bestWeCanAfford = i+1;

                        //we found a enemy cheap enough to spawn so break out of this loop after we spawn them
                        break;
                    }
                }
            }
        }

        //The rest is weak enemies
        int weakPoints = points - strongPoints - mediumPoints;

        remainingPoints += weakPoints; //carry over points from medium enemies
        bestWeCanAfford = weakEnemyPrefabs.Length;

        //as long as we have enough points to spawn the cheapest medium enemy try to find a random one to spawn
        while (remainingPoints >= weakEnemyPoints[0])
        {
            //pick a random weak enemy to look at in the range of cheapest to most expensive we were last able to spawn
            int randomIndex = Random.Range(0, bestWeCanAfford);

            int currentCost;
            if ((currentCost = weakEnemyPoints[randomIndex]) <= remainingPoints) //if we can afford it spawn them
            {
                //subtract the cost of this enemy from our remaining points
                remainingPoints -= currentCost;

                //spawn the enemy
                enemyDatabase.spawnEnemies(weakEnemyPrefabs[randomIndex], 1, spawnerNum % 3);
                spawnerNum++;
            }
            else //if we can't try to find a cheaper enemy (remember arrays should be sorted low to high in terms of cost)
            {
                for (int i = randomIndex; i < weakEnemyPrefabs.Length; i--)
                {
                    if ((currentCost = weakEnemyPoints[randomIndex]) <= remainingPoints)
                    {
                        //subtract the cost of this enemy from our remaining points
                        remainingPoints -= currentCost;

                        //spawn the enemy
                        enemyDatabase.spawnEnemies(weakEnemyPrefabs[i], 1, spawnerNum % 3);
                        spawnerNum++;

                        //this is now the MOST we could possibly afford (probably not even this tbh)
                        bestWeCanAfford = i + 1;

                        //we found a enemy cheap enough to spawn so break out of this loop after we spawn them
                        break;
                    }
                }
            }
        }
    }
}
