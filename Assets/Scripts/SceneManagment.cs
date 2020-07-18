using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    [SerializeField]
    Scenes currentScene = Scenes.OurMethod;
   
    public void Exit()
    {
        Application.Quit();
    }

    public void Train()
    {
        switch (currentScene)
        {
            case Scenes.OurMethod:
                SceneManager.LoadSceneAsync("OurMethodTrain");
                break;
            case Scenes.GestureType:
                SceneManager.LoadSceneAsync("GestureTypeTrain");
                break;
        }

    }

    public void StartExperiment()
    {
        switch (currentScene)
        {
            case Scenes.OurMethod:
                SceneManager.LoadSceneAsync("OurMethodMain");
                break;
            case Scenes.GestureType:
                SceneManager.LoadSceneAsync("GestureTypeMain");
                break;
        }
    }

    public void ContinueExperiment()
    {
        Scene current = SceneManager.GetActiveScene();
        switch (currentScene)
        {
            case Scenes.OurMethod:
                SceneManager.LoadSceneAsync("OurMethodMain");
                break;
            case Scenes.GestureType:
                SceneManager.LoadSceneAsync("GestureTypeMain");
                break;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void Setup()
    {
        SceneManager.LoadSceneAsync("Setup");
    }

    public enum Scenes
    {
        GestureType,
        OurMethod
    }
}
