using System.Collections;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private const string MAIN_MENU_SCENE_NAME = "MainMenu";
    
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
    }
    
}