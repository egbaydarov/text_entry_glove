﻿using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class MeasuringMetrics : MonoBehaviour
{

    [SerializeField]
    private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddGyMD2Db2wYOQC-Ix-lbYeeWJfT4t-gxE5TUgs9sYhSo5Sg/formResponse";

    [SerializeField]
    private EntryProcessing _currentEntryProcessing;

    public Stopwatch full_time { get; set; } = new Stopwatch();
    public Stopwatch entry_time_sw { get; set; } = new Stopwatch();
    public Stopwatch timer { get; set; } = new Stopwatch();

    [SerializeField]
    long _search_time;
    [SerializeField]
    long _entry_time;
    [SerializeField]
    long _remove_time;
    [SerializeField]
    long _check_time;
    [SerializeField]
    int _backspace_choose;
    [SerializeField]
    int _prediction_choose;
    [SerializeField]
    int _removed_count;

    public int prediction_choose
    {
        get => _prediction_choose;
        set
        {
            _prediction_choose = value;
        }
    }
    public int backspace_choose
    {
        get => _backspace_choose;
        set
        {
            _backspace_choose = value;
        }
    }
    public int removed_count
    {
        get => _removed_count;
        set
        {
            _removed_count = value;
        }
    }

    public long input_time
    {
        get => _search_time;
        set
        {
            _search_time = value;
        }
    }
    public long entry_time
    {
        get => _entry_time;
        set
        {
            _entry_time = value;
        }
    }
    public long remove_time
    {
        get => _remove_time;
        set
        {
            _remove_time = value;
        }
    }
    public long control_time
    {
        get => _check_time;
        set
        {
            _check_time = value;
        }
    }

    public bool isRemoves { get; set; } = false;
    public string sent_text { get; set; }

    volatile bool IsGestureExecuting = false;
    volatile bool IsEyeLastEnterInput = false;
    volatile bool IsEyeLastExitInput = false;
    volatile bool controlFlag = false;
    volatile bool inputFlag = false;


    private string _prevValue = "";


    Server server;

    private void Start()
    {
        if (!SceneManagment.isNew)
            LoadPrefs();
    }

    private void Update()
    {

    }

    private void OnApplicationQuit()
    {
        SavePrefs();
    }

    private void OnDestroy()
    {
        SavePrefs();
    }

    private void Awake()
    {
        server = FindObjectOfType<Server>();
    }

    public void SavePrefs()
    {
        if (SceneManagment.isMain)
        {
            PlayerPrefs.SetInt("Respondent_ID", (int)Settings.id); // Идентификатор испытуемого
            PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
            PlayerPrefs.SetInt("Attempt_number", _currentEntryProcessing.currentBlock); //Номер блока предложений
            PlayerPrefs.SetInt("Session_number", _currentEntryProcessing.currentSentence); //Номер попытки

            Debug.Log("Session Saved.");
        }
    }

    public void LoadPrefs()
    {
        if (SceneManagment.isMain)
        {
            if (PlayerPrefs.HasKey("InputMethod_ID"))
            {
                Settings.id = (uint)PlayerPrefs.GetInt("Respondent_ID");
                SceneManagment.method_id = PlayerPrefs.GetString("InputMethod_ID");
                _currentEntryProcessing.currentBlock = PlayerPrefs.GetInt("Attempt_number");
                _currentEntryProcessing.currentSentence = PlayerPrefs.GetInt("Session_number");

                Debug.Log(
                    $"Loaded: id {Settings.id}" +
                    $", method  {SceneManagment.method_id}" +
                    $", block {_currentEntryProcessing.currentBlock}" +
                    $", sentence {_currentEntryProcessing.currentSentence}");
            }
            else
            {
                Debug.Log("No saved sessions.");
            }
        }
    }

    public void DeletePrefs()
    {
        if (SceneManagment.isMain)
        {
            PlayerPrefs.DeleteKey("InputMethod_ID");
            PlayerPrefs.DeleteKey("Attempt_number");
            PlayerPrefs.DeleteKey("Session_number");
        }
    }



    public void ResetAll()
    {
        full_time.Reset();
        entry_time_sw.Reset();
        prediction_choose = 0;
        backspace_choose = 0;
        removed_count = 0;
        entry_time = 0;
        input_time = 0;
        remove_time = 0;
        control_time = 0;

        isRemoves = false;
    }



    public void OnCharacterRemoving(string value)
    {
        if (value == null)
            return;

        if (_currentEntryProcessing.LastTagDown.Equals("Backspace") && _prevValue.Length > value.Length)
        {
            isRemoves = true;
            removed_count += _prevValue.Length - value.Length;
        }

        _prevValue = value;
    }


    public void StartSentenceInput()
    {
        ResetAll();

        full_time.Restart();
        timer.Restart();
    }


    public void StartGesture()
    {
        IsGestureExecuting = true;

        timer.Stop();
        if (IsEyeLastEnterInput)
            input_time += timer.ElapsedMilliseconds;
        else
            control_time += timer.ElapsedMilliseconds;

        timer.Reset();
        timer.Start();

        entry_time_sw.Restart();
    }


    public void EndGesture()
    {
        IsGestureExecuting = false;

        entry_time_sw.Stop();
        entry_time += entry_time_sw.ElapsedMilliseconds;
        entry_time_sw.Reset();

        timer.Stop();
        if (!IsEyeLastEnterInput) //для исключения поиска первого во время заверешния росчерка
            control_time += timer.ElapsedMilliseconds;
        timer.Reset();
        timer.Start();
    }

    public void DeleteWord()
    {
        ++backspace_choose;

        timer.Stop();
        remove_time += timer.ElapsedMilliseconds;
        timer.Reset();

        timer.Start();
    }

    public void ChoosePrediction()
    {
        ++prediction_choose;
    }

    public void EndSentenceInput()
    {
        full_time.Stop();

        timer.Stop();
        if (IsEyeLastEnterInput)
            input_time += timer.ElapsedMilliseconds;
        else
            control_time += timer.ElapsedMilliseconds;

        timer.Reset();
    }

    public void OnInputEnter()
    {
        inputFlag = false;
        IsEyeLastEnterInput = true;

        timer.Stop();
        if (IsEyeLastExitInput && !controlFlag)
        {
            if (!IsGestureExecuting) //для исключения поиска первого во время росчерка
                input_time += timer.ElapsedMilliseconds;
        }
        else
            control_time += timer.ElapsedMilliseconds;
        timer.Reset();

        timer.Start();
    }

    public void OnControlEnter()
    {
        controlFlag = true;
        IsEyeLastEnterInput = false;

        timer.Stop();
        if (IsEyeLastExitInput || inputFlag)
        {
            if (!IsGestureExecuting) //для исключения поиска первого во время росчерка
                input_time += timer.ElapsedMilliseconds;
        }
        else
            control_time += timer.ElapsedMilliseconds;
        timer.Reset();

        timer.Start();
    }

    public void OnInputExit()
    {
        inputFlag = false;
        IsEyeLastExitInput = true;

        timer.Stop();
        if (!IsGestureExecuting) //для исключения поиска первого во время росчерка
            input_time += timer.ElapsedMilliseconds;
        timer.Reset();

        timer.Start();
    }

    public void OnControlExit()
    {
        controlFlag = false;
        IsEyeLastExitInput = false;

        timer.Stop();
        control_time += timer.ElapsedMilliseconds;
        timer.Reset();

        timer.Start();
    }
}