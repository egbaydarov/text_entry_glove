using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagment : MonoBehaviour
{
    [SerializeField]
    public Scenes currentScene = Scenes.GESTURE_TYPE;

    public static bool isMain { get; set; } = false;
    public static bool isNew { get; set; } = false;
    public static string method_id { get; set; }

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
            case Scenes.EYE_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "EYE_GAZE_AND_COMMIT";
                break;
            case Scenes.HEAD_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "HEAD_GAZE_AND_COMMIT";
                break;
            case Scenes.GESTURE_TYPE:
                SceneManager.LoadSceneAsync("GestureType_v2");
                method_id = "GESTURE_TYPE";
                break;
            case Scenes.OCULUS_QUEST:
                SceneManager.LoadSceneAsync("OculusQuest_v2");
                method_id = "OCULUS_QUEST";
                break;
            case Scenes.IMAGE_PLANE_POINTING:
                SceneManager.LoadSceneAsync("ImagePlanePointing");
                method_id = "IMAGE-PLANE_POINTING";
                break;
            case Scenes.ARTICULATED_HANDS:
                SceneManager.LoadSceneAsync("Articulatedhands_v2");
                method_id = "ARTICULATED_HANDS";
                break;
        }

    }

    public void StartExperiment()
    {
        Settings.id = (uint)PlayerPrefs.GetInt("Respondent_ID");
        isMain = true;
        isNew = true;
        PlayerPrefs.SetInt("Respondent_ID", (int)(++Settings.id));
        PlayerPrefs.Save();


        switch (currentScene)
        {
            case Scenes.EYE_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "EYE_GAZE_AND_COMMIT";
                break;
            case Scenes.HEAD_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "HEAD_GAZE_AND_COMMIT";
                break;
            case Scenes.GESTURE_TYPE:
                SceneManager.LoadSceneAsync("GestureType_v2");
                method_id = "GESTURE_TYPE";
                break;
            case Scenes.OCULUS_QUEST:
                SceneManager.LoadSceneAsync("OculusQuest_v2");
                method_id = "OCULUS_QUEST";
                break;
            case Scenes.IMAGE_PLANE_POINTING:
                SceneManager.LoadSceneAsync("ImagePlanePointing");
                method_id = "IMAGE-PLANE_POINTING";
                break;
            case Scenes.ARTICULATED_HANDS:
                SceneManager.LoadSceneAsync("Articulatedhands_v2");
                method_id = "ARTICULATED_HANDS";
                break;
        }
        PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
    }

    public void ContinueExperiment()
    {
        isMain = true;
        isNew = false;
        switch (currentScene)
        {
            case Scenes.EYE_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("GazeGesture");
                method_id = "EYE_GAZE_AND_COMMIT";
                break;
            case Scenes.HEAD_GAZE_AND_COMMIT:
                SceneManager.LoadSceneAsync("ReticleGesture");
                method_id = "HEAD_GAZE_AND_COMMIT";
                break;
            case Scenes.GESTURE_TYPE:
                SceneManager.LoadSceneAsync("GestureType_v2");
                method_id = "GESTURE_TYPE";
                break;
            case Scenes.OCULUS_QUEST:
                SceneManager.LoadSceneAsync("OculusQuest_v2");
                method_id = "OCULUS_QUEST";
                break;
            case Scenes.IMAGE_PLANE_POINTING:
                SceneManager.LoadSceneAsync("ImagePlanePointing");
                method_id = "IMAGE-PLANE_POINTING";
                break;
            case Scenes.ARTICULATED_HANDS:
                SceneManager.LoadSceneAsync("Articulatedhands_v2");
                method_id = "ARTICULATED_HANDS";
                break;
        }
        PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
    }


    public void LoadMenu()
    {
        isMain = false;
        isNew = false;
        SceneManager.LoadSceneAsync("MainMenu");
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
        EYE_GAZE_AND_COMMIT,
        HEAD_GAZE_AND_COMMIT,
        OCULUS_QUEST,
        GESTURE_TYPE,
        IMAGE_PLANE_POINTING,
        ARTICULATED_HANDS
    }
}
