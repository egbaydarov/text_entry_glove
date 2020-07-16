using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void Train()
    {
        SceneManager.LoadSceneAsync("TrainScene");
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void StartExperiment()
    {
        SceneManager.LoadSceneAsync("MainScene");
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void ContinueExperiment()
    {
        SceneManager.LoadSceneAsync("MainScene");
        SceneManager.UnloadSceneAsync("MainMenu");
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        SceneManager.UnloadSceneAsync("Setup");
    }

    public void Setup()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync("Setup");
        SceneManager.UnloadSceneAsync(current);
        
    }
}
