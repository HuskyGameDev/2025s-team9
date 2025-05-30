using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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
    private int raisedTimes = 0;
    private float income = 100; //Base income 
    public static float currency = 0;

    //Difficulty scale stuff
    private float diffMult = 1.25f; //Difficulty multiplier for taxes
    private int scale = 1; //Natural scale of enemy spawns (each tick is an extra enemy)
    int prevpoints; //Used to calcualte new amount of enemies needed
    int points; //Total amount of enemies to spawn
    int pointsKilled; //How many points of enemies the player has killed

    //Winning
    int soulsCount = 0;

    //Sounds
    public List<AudioClip> money = new List<AudioClip>();

    //Thomas's Temp Stuff
    private bool spawnEnemies = true;

    
    void Awake()
    {
        prevpoints = 10; //Set to base amount first
        //Sorry i know this is weird but it fixes an important bug
        currency = 0;
        state = State.build;
        currency += income;
        raisedTimes = 0;
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
    }

    void Intermission() {
        canBuild = false;
        buttons.SetActive(true);

    }

    public void IncreaseTaxes() {
        taxesRaised = true;
        raisedTimes++;
        points = calcEnemies();
        pointsKilled = 0;
        state = State.defend;
        buttons.SetActive(false);
    }

    public void KeepTaxes() {
        taxesRaised = false;
        points = calcEnemies();
        pointsKilled = 0;
        state = State.defend;
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

        if (soulsCount >= 150) {
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
            AudioController.Instance.PlaySound(money[UnityEngine.Random.Range(0, money.Count)]);
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
        SceneManager.LoadScene("LossScreen");
        gameObject.SetActive(false);
        //Time.timeScale = 0;
    }

    public void WinGame() {
        SceneManager.LoadScene("WinScreen");
        //gameObject.SetActive(false);
    }
}
