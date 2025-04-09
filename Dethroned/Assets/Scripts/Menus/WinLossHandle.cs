using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLossHandle : MonoBehaviour
{
    public void TitleScreen()
    {
        SceneManager.LoadScene("Root");
    }

    public void EndGame()
    {
        //TODO: Remove debug
        Debug.Log("Application Has been quit");
        Application.Quit();
    }
}
