using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    [SerializeField]
    Scenes currentScene = Scenes.OurMethod;

    public static string method_id;

    public void Start()
    {
        method_id = currentScene.ToString();
    }

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
        Settings.id++;
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
        MeasuringMetrics.LoadPrefs();
        Scene current = SceneManager.GetActiveScene();
        switch (currentScene)
        {
            case Scenes.OurMethod:
                SceneManager.LoadSceneAsync("OurMethodMain");
                method_id = "OurMethod";
                break;
            case Scenes.GestureType:
                SceneManager.LoadSceneAsync("GestureTypeMain");
                method_id = "GestureType";
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
