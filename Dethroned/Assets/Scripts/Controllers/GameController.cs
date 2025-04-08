using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    public GameObject buttons;
    public GameObject endBuildButton;
    public HUDController hudController;
    [SerializeField] WaveManager waveManager;
    //State related stuff
    public enum State {build, defend, intermission}; //States for state machine 
    public static State state = State.build; //Default into build state
    [HideInInspector]
    public static bool canBuild = true; // Hopefully helpful 

    //Income related stuff
    private bool taxesRaised = false; //Keep track if taxes were raised or not (also for difficulty)
    private float income = 100; //Base income 
    public static float currency;

    //Difficulty scale stuff
    private float diffMult = 1.25f; //Difficulty multiplier for taxes
    private int scale = 1; //Natural scale of enemy spawns (each tick is an extra enemy)
    int prevpoints; //Used to calcualte new amount of enemies needed
    int points; //Total amount of enemies to spawn
    int pointsKilled; //How many points of enemies the player has killed

    //Winning
    int soulsCount = 0;

    //Thomas's Temp Stuff
    private bool spawnEnemies = true;

    
    void Start()
    {
        prevpoints = 10; //Set to base amount first
        currency += income;
        UpdateHUD();
    }


    void Update()
    {
        //Check states and handle
        switch (state) {
            case State.build:
            Build();
            break;

            case State.intermission:
            Intermission();
            break;

            case State.defend:
            Defend();
            break;
        }
        UpdateHUD();
    }

    void Build() {
        canBuild = true;
        endBuildButton.SetActive(true);
    }

    public void EndBuild() {
        state = State.intermission;
        endBuildButton.SetActive(false);
        Debug.Log("Now choose to raise or lower taxes");
    }

    void Intermission() {
        canBuild = false;
        buttons.SetActive(true);
    }

    public void IncreaseTaxes() {
        taxesRaised = true;
        points = calcEnemies();
        pointsKilled = 0;
        Debug.Log(points);
        state = State.defend;
        Debug.Log("Taxes raised, now defending");
        buttons.SetActive(false);
    }

    public void KeepTaxes() {
        taxesRaised = false;
        points = calcEnemies();
        pointsKilled = 0;
        Debug.Log(points);
        state = State.defend;
        Debug.Log("Taxes not raised, now defending");
        buttons.SetActive(false);
    }

    int calcEnemies() {
        int enemies;
        if (taxesRaised) {
            enemies = (int)Math.Round((prevpoints + scale) * diffMult);
            prevpoints = enemies;
            return enemies;
        } else {
            enemies = prevpoints + scale;
            prevpoints = enemies;
            return enemies;
        }
    }

    void Defend() {
        canBuild = false;

        //Spawn enemies (in the if since this is called every frame, we don't want that many enemies)
        if (spawnEnemies)
        {
            spawnEnemies = false;
            waveManager.SpawnWave(points, .2f, .1f, .3f, .2f);
        }

        //TODO: change this to a more fitting value
        if (soulsCount >= 20) {
            WinGame();
        }

        if (points == pointsKilled) {
            spawnEnemies = true; //allow more enemies to be spawned next time we reach a defend state
            state = State.build;
            if (taxesRaised) {
                currency += (income + 50);
                
            } else {
                currency += income;
            }
            Debug.Log("Wave won");
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            EnemyDeath(1, 1);
        }
    }

    public void EnemyDeath(int souls, int empoints) {
        soulsCount += souls;
        pointsKilled += empoints;
    }

    public void UpdateHUD() {
    hudController.Money = (int) currency;
    hudController.Souls = soulsCount;
    }

    public void GameOver() {
        gameObject.SetActive(false);
        //Time.timeScale = 0;
    }

    public void WinGame() {
        //gameObject.SetActive(false);
    }
}
