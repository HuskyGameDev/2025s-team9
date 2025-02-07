using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTargetFinder : MonoBehaviour
{
    public enum TargetType { CLOSEST, STRONG, WEAK };

    //how this enemy will prioritize targets
    [SerializeField] TargetType targetType = TargetType.CLOSEST;

    //the target database (keeps track of what towers to attack)
    [SerializeField] private EnemyDatabase enemyDatabase = null; //TODO make this not serialized

    //the A* destination setting script that we need to pass a target to
    private AIDestinationSetter destSetter = null;

    //where this enemy spawned (default -1 so it throws an error if this isn't properly set)
    [SerializeField] private int spawnLocation = -1; //TODO: make this not serialized

    //this should only be called when the enemy is spawned in, it gives it the required info it needs on spawn
    public void setSpawnInfo(int spawnLocation, EnemyDatabase enemyDatabase)
    {
        this.enemyDatabase = enemyDatabase; 
        this.spawnLocation = spawnLocation;
    }

    private void Awake()
    {
        //get the destination setter of this enemy when it starts
        destSetter = this.GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        //give the enemy the latest target (TODO: might want to put this in something that runs slightly less frequently for performance reasons)
        destSetter.target = enemyDatabase.getNewTarget(targetType, spawnLocation);
    }
}
