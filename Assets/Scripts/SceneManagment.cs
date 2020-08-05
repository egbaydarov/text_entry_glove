using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    [SerializeField]
    Scenes currentScene = Scenes.OurMethod;

    public static bool isMain = false;
    public static string method_id = null;

    public void Start()
    {
        if(method_id==null)
            method_id = "test";

        if (!PlayerPrefs.HasKey("Respondent_ID"))
        {
            Settings.id = 0;
            PlayerPrefs.SetInt("Respondent_ID", (int) Settings.id);
        }

        else
        {
            Settings.id = (uint) PlayerPrefs.GetInt("Respondent_ID");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Train()
    {
        isMain = false;
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
        //EntryProcessing.ResetTime(); //TODO сделац отдеьный компонент для замера времени
        //isMain = true;
        MeasuringMetrics.LoadPrefs();
        Settings.id++;
        PlayerPrefs.SetInt("Respondent_ID", (int)Settings.id);
        
        EntryProcessing.currentBlock = 0;
        EntryProcessing.currentSentence = 0;
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

    public void ContinueExperiment()
    {
        //EntryProcessing.ResetTime(); //TODO сделац отдеьный компонент для замера времени
        //isMain = true;
        MeasuringMetrics.LoadPrefs();
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
        isMain = false;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync("MainMenu");
        SceneManager.UnloadSceneAsync(current);
    }

    public void Setup()
    {
        isMain = false;
        SceneManager.LoadSceneAsync("Setup");
    }

    public enum Scenes
    {
        GestureType,
        OurMethod
    }
}
