using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void OnClickPlay() {

        SceneManager.LoadScene("Assets/Scenes/Levels/Level_0/Level_0.unity");

    }

    public void OnClickQuit() {

        Application.Quit();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
}
