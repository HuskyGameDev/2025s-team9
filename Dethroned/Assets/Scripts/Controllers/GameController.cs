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
    private float diffMult = 1.25f; //Difficulty multiplier for taxes
    private int scale = 1; //Natural scale of enemy spawns (each tick is an extra enemy)
    int prevpoints; //Used to calcualte new amount of enemies needed
    int points; //Total amount of enemies to spawn
    int pointsKilled; //How many points of enemies the player has killed

    //Winning
    int soulsCount = 0;

    
    void Start()
    {
        prevpoints = 10; //Set to base amount first
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
        points = calcEnemies();
    }


    void Defend() {
        canBuild = false;
        //TODO: change this to a more fitting value
        if (soulsCount >= 20) {
            WinGame();
        }
        if (points == pointsKilled) {
            state = State.build;
            currency += income;
            Debug.Log("Wave won");
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            pointsKilled ++;
        }
    }


    int calcEnemies() {
        if (diffMult != 0) {
            return (int)Math.Round((prevpoints + scale) * diffMult);
        } else { 
            return prevpoints + scale;
        }
    }

    public void EnemyDeath(int souls, int empoints) {
        soulsCount += souls;
        pointsKilled += empoints;
    }

    public void GameOver() {
        gameObject.SetActive(false);
    }

    public void WinGame() {
        gameObject.SetActive(false);
    }

    
}
