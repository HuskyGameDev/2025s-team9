using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Root"))
        {
            SceneManager.LoadScene("Assets/Scenes/MainMenu.unity", LoadSceneMode.Additive);
            //SceneManager.SetActiveScene(SceneManager.GetSceneByPath("Assets/Scenes/MainMenu.unity"));
        }
        
    }
}
