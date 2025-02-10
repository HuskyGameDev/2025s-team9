using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //WIP: This is just a quick thing to call from a UI so I can test the enemy database/target finder stuff

    [SerializeField] EnemyDatabase enemyDatabase;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] int enemySpawnCount;
    [SerializeField] int spawnerNum; //this is only the initial spawner location

    public void spawnEnemy()
    {
        enemyDatabase.updateSpawnerListings(null);
        enemyDatabase.updateTargetDatabase(null, false);
        enemyDatabase.spawnEnemies(enemyPrefabs[spawnerNum%3], enemySpawnCount, spawnerNum%3);

        //this way each click will spawn at different spawner
        spawnerNum++;
    }
}
