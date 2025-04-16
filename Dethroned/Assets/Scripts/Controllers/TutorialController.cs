using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public TextMeshProUGUI textbox;
    public GameObject text;
    public GameObject next;
    public GameObject hp;
    public GameObject money;
    public GameObject souls;
    public GameObject stowers;
    public GameObject target;
    public GameObject end;
    public GameObject endButton;
    private int stage = 0;
    private string[] tutorialText = new string[13];

    private void Awake()
    {
        stage = 0;
        tutorialText[0] = "Welcome to Dethroned, your goal in this game is to place towers to defend your castle from the peasants rebelling.";
        tutorialText[1] = "This is your health bar, when it becomes red, your castle will be destroyed and the game will end.";
        tutorialText[2] = "This is your current money, you need money to build towers";
        tutorialText[3] = "These are your souls, you get them by defeating enemies. Once you collect a sufficent amount, you will win the game.";
        tutorialText[4] = "Dethroned is split into 3 phases, the first lets you build towers.";
        tutorialText[5] = "You can click here to see what towers you can build";
        tutorialText[6] = "You can use this drop down menu to select this towers target priority";
        tutorialText[7] = "You can click anywhere to place the tower";
        tutorialText[8] = "You can click here to end the build phase";
        tutorialText[9] = "You can then choose to raise taxes, or keep them the same. If you raise taxes your village will get more angry and you will have a more difficult round";
        tutorialText[10] = "Please note, keeping taxes the same does not lessen your village's anger, so chose carefully!";
        tutorialText[11] = "You will then go into a defend state where enemies will spawn, once all enmemies are dead the wave is over";
        tutorialText[12] = "Good Luck and enjoy Dethroned!";
    }

    private void Update()
    {
        switch (stage)
        {
            case 0:
                textbox.text = tutorialText[0];
                break;
            case 1:
                textbox.text = tutorialText[1];
                hp.SetActive(true);
                break;
            case 2:
                textbox.text = tutorialText[2];
                hp.SetActive(false);
                money.SetActive(true);
                break;
            case 3:
                textbox.text = tutorialText[3];
                money.SetActive(false);
                souls.SetActive(true);
                break;
            case 4:
                textbox.text = tutorialText[4];
                souls.SetActive(false);
                break;
            case 5:
                textbox.text = tutorialText[5];
                stowers.SetActive(true);
                break;
            case 6:
                textbox.text = tutorialText[6];
                stowers.SetActive(false);
                target.SetActive(true);
                break;
            case 7:
                textbox.text = tutorialText[7];
                target.SetActive(false);
                break;
            case 8:
                textbox.text = tutorialText[8];
                end.SetActive(true);
                endButton.SetActive(true);
                break;
            case 9:
                textbox.text = tutorialText[9];
                end.SetActive(false);
                endButton.SetActive(false);
                break;
            case 10:
                textbox.text = tutorialText[10];
                break;
            case 11:
                textbox.text = tutorialText[11];
                break;
            case 12:
                textbox.text = tutorialText[12];
                break;
        }
    }

    public void NextPhase()
    {
        if (stage < 12)
        {
            stage++;
        } else
        {
            SceneManager.LoadScene("Root");
        }
    }


}
