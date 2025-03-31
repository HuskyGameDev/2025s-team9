using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

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
    [SerializeField] public GameObject[] spawners = null;

    //How far to look for potential targets
    [SerializeField] float searchDistance = 0;

    //Store the nearest of each gameobject to pass onto enemies quickly (only update these when the updateTargetDatabase method is called)
    //The indicies correspond to how many spawners there are (and their number)
    private GameObject[] nearestStrong;
    private GameObject[] nearestWeak;
    private GameObject[] nearestTarget;
    
    //This updates the listings of spawners and their corresponding target arrays WARNING: this must be called before updateTargetDatabase
    //If you're not adding a spawn location just pass null (do this to instantiate the target arrays if you're just setting spawner locations through the editor)
    public void updateSpawnerListings(GameObject spawnLocation)
    {
        //add a new spawn location
        if(spawnLocation != null)
        {
            //TODO: do stuff here
        }

        nearestStrong = new GameObject[spawners.Length];
        nearestWeak = new GameObject[spawners.Length];
        nearestTarget = new GameObject[spawners.Length];
    }

    //Update the target database with the objects id to remove or add it, call this to kill towers
    //This should be called when towers are placed or destroyed
    public void updateTargetDatabase(GameObject updatedTarget, bool remove)
    {
        //if remove is true take the object out of the database and then destroy it
        if(remove && updatedTarget != null)
            Destroy(updatedTarget);

        //update various target listings
        for (int i = 0; i < spawners.Length; i++)
        {
            categorizeTowers(i);
        }
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
    public void spawnEnemies(GameObject enemyPrefab, int count, int spawnerNum, int wavePoints, int soulPoints)
    {
        for (int i = 0; i < count; i++)
        {
            //spawn enemy, have their location offset ever so slightly so the physics trigger and push them apart (otherwise they'll all be inside each other)
            GameObject newSpawn = Instantiate(enemyPrefab, spawners[spawnerNum].transform.position + new Vector3(0.0001f*i,0.0001f*i,0), Quaternion.identity);

            //set spawn info - spawn location, enemy database (this script)
            newSpawn.GetComponent<EnemyTargetFinder>().setSpawnInfo(spawnerNum, this.GetComponent<EnemyDatabase>());
            newSpawn.GetComponent<EnemyStats>().SetSpawnInfo(wavePoints, soulPoints);
        }
    }

    //finds the different target types for a given spawner
    private void categorizeTowers(int spawnerNum)
    {
        //TODO: probably can fix this by assigning the castle as tower 0
        if (TowerBuilder.Instance.ActiveTowers().Count <= 0)
            Debug.LogError("No towers in the scene");


        GameObject strongTower = null;
        float strongValue = -1;

        GameObject weakTower = null;
        float weakValue = 1000000000000;
        
        GameObject closeTower = null;
        float closeValue = 10000000000;

        //iterate through each and store index and value associated with each
        for (int i = 0; i < spawners.Length; i++)
        {
            float analyzedValue = 0;

            //look at every tower relative to every spawn point
            foreach (GameObject tower in TowerBuilder.Instance.ActiveTowers())
            {

                //find the strongest tower
                if ((analyzedValue = tower.GetComponent<Tower>().MaxHealth) > strongValue)
                {
                    strongTower = tower;
                    strongValue = analyzedValue;
                }

                //find the weakest tower
                if ((analyzedValue = tower.GetComponent<Tower>().MaxHealth) < weakValue)
                {
                    weakTower = tower;
                    weakValue = analyzedValue;
                }

                //find the closest tower
                analyzedValue = (spawners[i].transform.position - tower.gameObject.transform.position).magnitude;
                if (analyzedValue < closeValue)
                {
                    closeTower = tower;
                    closeValue = analyzedValue;
                }
            }
        }

        nearestStrong[spawnerNum] = strongTower;
        nearestWeak[spawnerNum] = weakTower;
        nearestTarget[spawnerNum] = closeTower;
    }
}
