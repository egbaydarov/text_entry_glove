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
    public static bool isNew = false;
    public static string method_id = null;

    public void Start()
    {

        if (method_id == null)
            method_id = "test";
        if (!PlayerPrefs.HasKey("Respondent_ID"))
        {
            Settings.id = 0;
            PlayerPrefs.SetInt("Respondent_ID", (int)Settings.id);
        }
        else
        {
            Settings.id = (uint)PlayerPrefs.GetInt("Respondent_ID");
        }
        PlayerPrefs.Save();
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
            case Scenes.OculusQuest:
                SceneManager.LoadSceneAsync("OculusQuestTrain");
                break;
            case Scenes.PointMethod:
                SceneManager.LoadSceneAsync("PointMethodTrain");
                break;
            case Scenes.GazeCharacter:
                SceneManager.LoadSceneAsync("GazeCharacter");
                method_id = "CHARACTER_GAZE";
                break;
            case Scenes.GazeGesture:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "GESTURE_GAZE";
                break;
            case Scenes.ReticleCharacter:
                SceneManager.LoadSceneAsync("ReticleCharacter");
                method_id = "CHARACTER_RETICLE";
                break;
            case Scenes.ReticleGesture:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "GESTURE_RETICLE";
                break;
        }

    }

    public void StartExperiment()
    {
        Settings.id = (uint)PlayerPrefs.GetInt("Respondent_ID");
        isMain = true;
        isNew = true;
        PlayerPrefs.SetInt("Respondent_ID", (int)(++Settings.id));


        for (int i = 0; i < 64; ++i) //TODO сделать красиво
        {
            PlayerPrefs.DeleteKey($"SentenceOrder{i}");
        }

        PlayerPrefs.Save();
        Debug.Log("Delete saved sentences order");


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
            case Scenes.OculusQuest:
                SceneManager.LoadSceneAsync("OculusQuestMain");
                method_id = "OculusQuest";
                break;
            case Scenes.PointMethod:
                SceneManager.LoadSceneAsync("PointMethodMain");
                method_id = "PontMethod";
                break;
            case Scenes.GazeCharacter:
                SceneManager.LoadSceneAsync("GazeCharacter");
                method_id = "CHARACTER_GAZE";
                break;
            case Scenes.GazeGesture:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "GESTURE_GAZE";
                break;
            case Scenes.ReticleCharacter:
                SceneManager.LoadSceneAsync("ReticleCharacter");
                method_id = "CHARACTER_RETICLE";
                break;
            case Scenes.ReticleGesture:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "GESTURE_RETICLE";
                break;
        }
    }

    public void ContinueExperiment()
    {
        isMain = true;
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
            case Scenes.OculusQuest:
                SceneManager.LoadSceneAsync("OculusQuestMain");
                method_id = "OculusQuest";
                break;
            case Scenes.PointMethod:
                SceneManager.LoadSceneAsync("PointMethodMain");
                method_id = "PontMethod";
                break;
            case Scenes.GazeCharacter:
                SceneManager.LoadSceneAsync("GazeCharacter");
                method_id = "CHARACTER_GAZE";
                break;
            case Scenes.GazeGesture:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "GESTURE_GAZE";
                break;
            case Scenes.ReticleCharacter:
                SceneManager.LoadSceneAsync("ReticleCharacter");
                method_id = "CHARACTER_RETICLE";
                break;
            case Scenes.ReticleGesture:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "GESTURE_RETICLE";
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

    private void Awake()
    {

        SceneManagment[] objs = FindObjectsOfType<SceneManagment>();
        if (objs.Length > 1)
        {
            enabled = false;
        }
    }

    public void Setup()
    {
        isMain = false;
        SceneManager.LoadSceneAsync("Setup");
    }

    public enum Scenes
    {
        GestureType,
        OurMethod,
        OculusQuest,
        PointMethod,
        GazeCharacter,
        GazeGesture,
        ReticleCharacter,
        ReticleGesture
    }
}
