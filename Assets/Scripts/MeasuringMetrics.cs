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
    public static Stopwatch receive_time = new Stopwatch();
    public static Stopwatch end_time = new Stopwatch();
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
        
       //Debug.Log("Start Gesture");
       //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
        if (!all_time.IsRunning)
            all_time.Start();
        gest_time.Start();
       //Debug.Log("gest time start");
        all_move_time += ((float) move_time.ElapsedMilliseconds) / 1000;
        move_time.Reset();
        //Debug.Log("move time added and reset");
        if (wasChosen)
        {
            all_choose_time += (float)end_time.ElapsedMilliseconds/1000 - ((float) choose_time.ElapsedMilliseconds) / 1000;
            all_fix_choose_time += (float)receive_time.ElapsedMilliseconds/1000 - ((float) choose_time.ElapsedMilliseconds) / 1000;
            choose_time.Reset();
            receive_time.Reset();
            end_time.Reset();
            //Debug.Log("choose time added and reset");
            //Debug.Log("fix choose time added and reset");
            wasChosen = false;
        }
        
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
    }

    public static void EndGesture()
    {
        //Debug.Log("End Gesture");
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
        move_time.Start();
        //Debug.Log("move time start");
        wait_time.Start();
        //Debug.Log("wait time start");
        gest_time.Stop();
        //Debug.Log("gest time stop");
        end_time.Restart();
        //Debug.Log("choose time restart");
        
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
    }

    public static void ReceivePredictions()
    {
        Debug.Log("Receive Predictions");
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
        wait_time.Stop();
        //Debug.Log("wait time stop");
        
        //Debug.Log("fix choose time restart");
        
        
        receive_time.Restart();
        
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
           }

    public static void ChoosePredictions()
    {
        Debug.Log("Choose Prediction");
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
         move_time.Restart();
        //Debug.Log("move time restart");
        choose_time.Restart();
        //Debug.Log("choose time stop");
       
        wasChosen = true;
        
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");

    }

    public static void Finish()
    {
        //Debug.Log("Finish block");
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
         all_move_time += ((float) move_time.ElapsedMilliseconds) / 1000;
        move_time.Reset();
        //Debug.Log("move time added and reset");
        if (wasChosen)
        {
            all_choose_time += (float)end_time.ElapsedMilliseconds/1000 - ((float) choose_time.ElapsedMilliseconds) / 1000;
            all_fix_choose_time += (float)receive_time.ElapsedMilliseconds/1000 - ((float) choose_time.ElapsedMilliseconds) / 1000;
            receive_time.Restart();
            choose_time.Reset();
            end_time.Reset();
        //    Debug.Log("fix time added and reset");
        //    Debug.Log("all fix time added and reset");
            wasChosen = false;
        //    Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");

        }
    }

    public static void ResetTime()
    {
        //Debug.Log("Reset Time");
        
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
        all_time.Reset();
        move_time.Reset();
        gest_time.Reset();
        choose_time.Reset();
        fix_choose_time.Reset();
        wait_time.Reset();
        receive_time.Reset();
        end_time.Reset();
        all_move_time = 0;
        all_choose_time = 0;
        all_fix_choose_time = 0;
        //Debug.Log($"all time: {(float)all_time.ElapsedMilliseconds/1000}\n all move time: {all_move_time}, one move time: {(float)move_time.ElapsedMilliseconds/1000} \n all choose time: {all_choose_time}, one choose time: {(float)choose_time.ElapsedMilliseconds/1000} \n all fix choose time: {all_fix_choose_time}, one fix choose time: {(float)fix_choose_time.ElapsedMilliseconds/1000} \n gest time: {(float)gest_time.ElapsedMilliseconds/1000}\n wait time: {(float)wait_time.ElapsedMilliseconds/1000}");
        
    }

}