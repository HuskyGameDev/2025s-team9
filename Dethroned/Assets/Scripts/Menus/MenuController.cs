using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(menu.scene);
        if (SceneManager.GetActiveScene() == menu.scene)
        {
            Debug.Log("Success");
        }
        else { Debug.Log("Fail"); }
    }

    public void OpenScene(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(menu.scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
