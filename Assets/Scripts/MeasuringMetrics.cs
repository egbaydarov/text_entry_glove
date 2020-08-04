using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class MeasuringMetrics : MonoBehaviour
{

    [SerializeField] private string FORM_URL =
        "https://docs.google.com/forms/u/0/d/e/1FAIpQLScWlRx70e3SICI3YnnIJSOVPJ3jGoORoAdh-NvsTnuTtVpqkw/formResponse";

    [SerializeField] private InputField intext;
    [SerializeField] private GameObject reticlePointer;
    private int block_num;
    private int sent_num;

    private string sent_text;

    // private float all_time;
    // private float gest_time;
    // private float move_time;
    private int text_length;


    public static Stopwatch all_time = new Stopwatch();
    public static Stopwatch move_time = new Stopwatch();
    public static Stopwatch gest_time = new Stopwatch();
    public static Stopwatch choose_time = new Stopwatch();
    public static Stopwatch fix_choose_time = new Stopwatch();
    public static Stopwatch wait_time = new Stopwatch();
    public static float all_move_time = 0;
    public static float all_choose_time = 0;
    public static float all_fix_choose_time = 0;

    private static bool wasChosen = false;

    Server server;

    // Start is called before the first frame update
    void Start()
    {
        //Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void SavePrefs()
    {
        PlayerPrefs.SetInt("Respondent_ID", (int) Settings.id); // Идентификатор испытуемого
        PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
        PlayerPrefs.SetInt("Attempt_number", EntryProcessing.currentBlock); // Номер блока предложений
        PlayerPrefs.SetInt("Session_number", EntryProcessing.currentSentence); // Номер попытки

        Debug.Log("Saved");
    }

    public static void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("InputMethod_ID"))
        {
            Settings.id = (uint) PlayerPrefs.GetInt("Respondent_ID");
            SceneManagment.method_id = PlayerPrefs.GetString("InputMethod_ID");
            EntryProcessing.currentBlock = PlayerPrefs.GetInt("Attempt_number");
            EntryProcessing.currentSentence = PlayerPrefs.GetInt("Session_number");

            Debug.Log(
                $"Loaded : id {Settings.id}, method  {SceneManagment.method_id}, block {EntryProcessing.currentBlock}, sentence {EntryProcessing.currentSentence}");
        }
        else
            Debug.Log("No saved prefs");
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteKey("InputMethod_ID");
        PlayerPrefs.DeleteKey("Attempt_number");
        PlayerPrefs.DeleteKey("Session_number");
    }

    public static void StartGesture()
    {
        
        Debug.Log("Start Gesture");
        if (!all_time.IsRunning)
            all_time.Start();
        gest_time.Start();
        all_move_time += ((float) move_time.ElapsedMilliseconds) / 1000;
        move_time.Reset();
        if (wasChosen)
        {
            all_choose_time += ((float) choose_time.ElapsedMilliseconds) / 1000;
            all_fix_choose_time += ((float) fix_choose_time.ElapsedMilliseconds) / 1000;
            choose_time.Reset();
            fix_choose_time.Reset();
            wasChosen = false;
        }

    }

    public static void EndGesture()
    {
        Debug.Log("End Gesture");
        move_time.Start();
        wait_time.Start();
        gest_time.Stop();
        choose_time.Restart();
    }

    public static void ReceivePredictions()
    {
        Debug.Log("Receive Predictions");
        wait_time.Stop();
        fix_choose_time.Restart();
    }

    public static void ChoosePredictions()
    {
        Debug.Log("Choose Prediction");
        move_time.Restart();
        choose_time.Stop();
        wasChosen = true;
    }

    public static void Finish()
    {
        Debug.Log("Finish block");
        all_move_time += ((float) move_time.ElapsedMilliseconds) / 1000;
        move_time.Reset();
        if (wasChosen)
        {
            all_choose_time += ((float) choose_time.ElapsedMilliseconds) / 1000;
            all_fix_choose_time += ((float) fix_choose_time.ElapsedMilliseconds) / 1000;
            choose_time.Reset();
            fix_choose_time.Reset();
            wasChosen = false;
        }
    }

    public static void ResetTime()
    {
        Debug.Log("Reset Time");
        all_time.Reset();
        move_time.Reset();
        gest_time.Reset();
        choose_time.Reset();
        fix_choose_time.Reset();
        wait_time.Reset();
        all_move_time = 0;
        all_choose_time = 0;
        all_fix_choose_time = 0;
    }

}