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
    [SerializeField] EnemyDatabase enemyDatabase = null;

    //the A* destination setting script that we need to pass a target to
    private AIDestinationSetter destSetter = null;

    //where this enemy spawned (probably could just be this objects parent?)
    private GameObject spawnLocation = null;

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
