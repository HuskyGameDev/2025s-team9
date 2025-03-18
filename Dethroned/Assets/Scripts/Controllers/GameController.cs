using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    //State related stuff
    public enum State {build, defend, intermission}; //States for state machine 
    public static State state = State.build; //Default into build state
    [HideInInspector]
    public static bool canBuild = true; // Hopefully helpful 

    //Income related stuff
    private bool taxesRaised = false; //Keep track if taxes were raised or not (also for difficulty)
    private int income = 100; //Base income 
    public static float currency;

    //Difficulty scale stuff
    private float diffMult = 0; //Difficulty multiplier for taxes
    private int scale = 1; //Natural scale of enemy spawns (each tick is an extra enemy)
    int prevEnemiesLeft; //Used to calcualte new amount of enemies needed
    int enemiesLeft; //Keeps track of current enemies left

    //Winning
    int soulsCount = 0;

    
    void Start()
    {
        prevEnemiesLeft = 10; //Set to base amount first
        currency = income;
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
    }

    void Build() {
        canBuild = true;
        if (Input.GetKeyDown(KeyCode.F)) {
            state = State.intermission;
            Debug.Log("Now choose to raise or lower taxes");
            enemiesLeft = calcEnemies();
        }
    }

    void Intermission() {
        canBuild = false;
        if (Input.GetKeyDown(KeyCode.G)) {
            taxesRaised = true;
            income += 100;
            state = State.defend;
            Debug.Log("Taxes raised, now defending");
        } else if (Input.GetKeyDown(KeyCode.H)) {
            taxesRaised = false;
            state = State.defend;
            Debug.Log("Taxes not raised, now defending");
        }
    }


    void Defend() {
        canBuild = false;
        //TODO: change this to a more fitting value
        if (soulsCount >= 20) {
            WinGame();
        }
        if (enemiesLeft == 0) {
            state = State.build;
            currency += income;
            Debug.Log("Wave won");
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            enemiesLeft --;
        }
    }


    int calcEnemies() {
        if (diffMult != 0) {
            return (int)Math.Round((prevEnemiesLeft + scale) * diffMult);
        } else { 
            return prevEnemiesLeft + scale;
        }
    }

    public void EnemyDeath(int souls, int points) {
        soulsCount += souls;
        enemiesLeft -= points;
    }

    public void GameOver() {
        //LevelManager.instance.GameOver();
        gameObject.SetActive(false);
    }

    public void WinGame() {
        gameObject.SetActive(false);
    }

    
}
