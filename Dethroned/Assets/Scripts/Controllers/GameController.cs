using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    //State related stuff
    private enum State {build, defend, intermission}; //States for state machine 
    State state = State.build; //Default into build state
    private bool canBuild = true; // Hopefully helpful 

    //Income related stuff
    private bool taxesRaised = false; //Keep track if taxes were raised or not (also for difficulty)
    private int income = 100; //Base income 

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
        if (enemiesLeft == 0) {
            state = State.build;
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

    
}
