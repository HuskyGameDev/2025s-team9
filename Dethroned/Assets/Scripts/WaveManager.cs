using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    //WIP: This is just a quick thing to call from a UI so I can test the enemy database/target finder stuff

    [SerializeField] EnemyDatabase enemyDatabase;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int enemySpawnCount;
    [SerializeField] int spawnerNum;

    public void spawnEnemy()
    {
        enemyDatabase.spawnEnemies(enemyPrefab, enemySpawnCount, spawnerNum);
    }
}
