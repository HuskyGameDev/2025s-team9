using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This acts as a database for giving enemies targets as well as a way of spawning them in
public class EnemyDatabase : MonoBehaviour
{
    /*
     * 
     * This might end up being pretty laggy so it might be better to register a database of sorts of all the players towers each wave and have enemies target based on stuff there (so they aren't all calculating ranges/strengths)
     * Could update that database anytime a tower gets destroyed
     * 
     * Could maybe calculate closest thing from each spawnpoint to direct enemies that way (so they don't all target one area but also aren't all individually calculating stuff)
     * 
     * On enemy when their target becomes null (they destroyed it) request a new one from the nearest spawner, could have the spawners be like control towers (commander knights or something)
     * 
     */

    //A list of objects to act as spawn locations for enemies
    [SerializeField] private GameObject[] spawners = null;

    //How far to look for potential targets
    [SerializeField] float searchDistance = 0;

    //Store the nearest of each gameobject to pass onto enemies quickly (only update these when the updateTargetDatabase method is called)
    //The indicies correspond to how many spawners there are (and their number)
    private GameObject[] nearestStrong = null;
    private GameObject[] nearestWeak = null;
    [SerializeField] private GameObject[] nearestTarget = null; //TODO: this shouldn't be serialized (probably)

    //Update the target database with the objects id to remove or add it, call this to kill towers
    public void updateTargetDatabase(GameObject updatedTarget, bool remove)
    {
        //if remove is true take the object out of the database and then destroy it
        Destroy(updatedTarget);

        //update various target listings
    }

    //return a new target according to the given target type and the objects spawn location
    public Transform getNewTarget(EnemyTargetFinder.TargetType tType, int spawnLocation)
    {
        switch(tType)
        {
            case EnemyTargetFinder.TargetType.CLOSEST:
                return nearestTarget[spawnLocation].transform;

            case EnemyTargetFinder.TargetType.STRONG:
                return nearestStrong[spawnLocation].transform;

            case EnemyTargetFinder.TargetType.WEAK:
                return nearestWeak[spawnLocation].transform;
        }

        //something went wrong
        return null;
    }

    //Spawn "count" amount of the given enemy type at the given spawner (TODO: maybe change them to have strings for names?)
    public void spawnEnemies(GameObject enemyPrefab, int count, int spawner)
    {
        //spawn enemy
        GameObject newSpawn = Instantiate(enemyPrefab);

        //set spawn info - spawn location, enemy database (this script)
        newSpawn.GetComponent<EnemyTargetFinder>().setSpawnInfo(spawner, this.GetComponent<EnemyDatabase>());
    }
}
