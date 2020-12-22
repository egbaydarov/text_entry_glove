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

    [SerializeField]
    private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddGyMD2Db2wYOQC-Ix-lbYeeWJfT4t-gxE5TUgs9sYhSo5Sg/formResponse";

    [SerializeField]
    private EntryProcessing _currentEntryProcessing;

    public Stopwatch full_time { get; set; } = new Stopwatch();
    public Stopwatch search_time_sw { get; set; } = new Stopwatch();
    public Stopwatch search_time_sw_single { get; set; } = new Stopwatch();
    public Stopwatch entry_time_sw { get; set; } = new Stopwatch();
    public Stopwatch entry_time_sw_single { get; set; } = new Stopwatch();
    public Stopwatch remove_time_sw { get; set; } = new Stopwatch();
    public Stopwatch check_time_sw { get; set; } = new Stopwatch();
    public Stopwatch check_time_sw_eye { get; set; } = new Stopwatch();

    [SerializeField]
    long _search_time;
    [SerializeField]
    long _entry_time;
    [SerializeField]
    long _remove_time;
    [SerializeField]
    long _check_time;
    [SerializeField]
    long _check_time_eye;
    [SerializeField]
    long _entry_time_single;
    [SerializeField]
    long _search_time_single;
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
    public bool HasWrong { get; set; }
    public long search_time
    {
        get => _search_time;
        set
        {
            _search_time = value;
        }
    }
    public long search_time_single
    {
        get => _search_time_single;
        set
        {
            _search_time_single = value;
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
    public long entry_time_single
    {
        get => _entry_time_single;
        set
        {
            _entry_time_single = value;
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
    public long check_time
    {
        get => _check_time;
        set
        {
            _check_time = value;
        }
    }
    public long check_time_eye
    {
        get => _check_time_eye;
        set
        {
            _check_time_eye = value;
        }
    }
    public bool isRemoves { get; set; } = false;
    public bool RemoveFlag { get; set; } = false;
    public string sent_text { get; set; }


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
        PlayerPrefs.SetInt("Respondent_ID", (int)Settings.id); // Идентификатор испытуемого
        PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
        PlayerPrefs.SetInt("Attempt_number", _currentEntryProcessing.currentBlock); //Номер блока предложений
        PlayerPrefs.SetInt("Session_number", _currentEntryProcessing.currentSentence); //Номер попытки

        Debug.Log("Session Saved.");
    }

    public void LoadPrefs()
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

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteKey("InputMethod_ID");
        PlayerPrefs.DeleteKey("Attempt_number");
        PlayerPrefs.DeleteKey("Session_number");
    }

    public void StartSentenceInput()
    {
        ResetAll();
        full_time.Start();
        entry_time_sw_single.Start();

    }

    public void ResetAll()
    {
        full_time.Reset();
        search_time_sw.Reset();
        search_time_sw_single.Reset();
        entry_time_sw_single.Reset();
        entry_time_sw.Reset();
        remove_time_sw.Reset();
        check_time_sw.Reset();
        check_time_sw_eye.Reset();
        prediction_choose = 0;
        backspace_choose = 0;
        removed_count = 0;
        entry_time = 0;
        entry_time_single = 0;
        search_time = 0;
        search_time_single = 0;
        remove_time = 0;
        check_time = 0;
        check_time_eye = 0;

        //check_time = 0; //TODO
        isRemoves = false;
        HasWrong = false;
    }

    public void EndSentenceInput()
    {
        full_time.Stop();
    }

    public void OnCharacterRemoving(string value)
    {
        if (value == null)
            return;

        if (_currentEntryProcessing.LastTagDown.Equals("Backspace") && _prevValue.Length > value.Length)
        {
            if (value.Length != 0 && value[value.Length - 1] != ' ')
                HasWrong = true;
            isRemoves = true;
            removed_count += _prevValue.Length - value.Length;
        }

        _prevValue = value;
    }
    

    
}