using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TrialDataStorage : MonoBehaviour
{

    [Serializable]
    private struct SerializableWrapper
    {
        public SerializableWrapper(Queue<TrialData> allTrialsData)
        {
            AllTrialsData = allTrialsData.ToArray();
        }

        public TrialData[] AllTrialsData;
    }


    private Queue<TrialData> _storedTrialData { get; set; }
    private TrialData _currentTrialData;

    [SerializeField]
    TextHelper TextHelper;

    [SerializeField]
    private EntryProcessing _currentEntryProcess;

    [SerializeField]
    private MeasuringMetrics _measuringMetrics;

    private const string FILE_NAME = "/AllTrialData.json";

    private void Start()
    {
        //Save();
        if (IsThereUnsavedData())
            StartCoroutine(TryToSaveToGoogleSheets());
    }

    void Awake()
    {
        try
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + FILE_NAME, System.Text.Encoding.UTF8);
            string json = reader.ReadToEnd();
            if (json.Length > 0)
            {
                SerializableWrapper data = JsonUtility.FromJson<SerializableWrapper>(json);
                _storedTrialData = new Queue<TrialData>(data.AllTrialsData);
            }
            else
                _storedTrialData = new Queue<TrialData>();

            TextHelper = FindObjectOfType<TextHelper>();

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            _storedTrialData = new Queue<TrialData>();
        }
    }

    void OnDestroy()
    {
        if (IsThereUnsavedData())
            SaveEverythingToLocalStorage();
    }

    public TrialData GetCurrectTrialData()
    {
        return _currentTrialData;
    }

    public bool IsThereUnsavedData()
    {
        return (_storedTrialData.Count > 0);
    }

    public void NextTrial()
    {
        //// Fool proffing
        //if (_currentTrialData != null)
        //    _storedTrialData.Enqueue(_currentTrialData);

        _currentTrialData = new TrialData()
        {
            block_num = (_currentEntryProcess.currentBlock + 1).ToString(),
            sent_num = (_currentEntryProcess.currentSentence + 1).ToString(),
            sent_text = _measuringMetrics.sent_text,
            all_time = _measuringMetrics.full_time.ElapsedMilliseconds / 1000f,
            removed_count = _measuringMetrics.removed_count.ToString(),
            backspace_count = _measuringMetrics.backspace_choose.ToString(),
            prediction_count = _measuringMetrics.prediction_choose.ToString(),
            search_time = (_measuringMetrics.search_time / 1000f).ToString(),
            entry_time = (_measuringMetrics.entry_time / 1000f).ToString(),
            removing_time = _measuringMetrics.isRemoves ? (_measuringMetrics.remove_time / 1000f).ToString() : "",
            HasError = _measuringMetrics.HasWrong,
            resp_text = TextHelper.text,
            check_time = (_measuringMetrics.check_time / 1000f).ToString()
        };

        

        if(FindObjectsOfType<EyeMetrics>().Length != 0)
        {
            _currentTrialData.check_time = (_measuringMetrics.check_time_eye / 1000f).ToString();
            _currentTrialData.search_time = (_measuringMetrics.search_time_eye / 1000f).ToString();
        }

    }


    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        if (_currentTrialData != null)
        {
            _storedTrialData.Enqueue(_currentTrialData);
            _currentTrialData = null;
            Debug.Log("Saved current Trial Data");
        }

        //Debug.Log("NOT saved current Trial Data");
        if (IsThereUnsavedData())
            StartCoroutine(TryToSaveToGoogleSheets());
    }

    public void Save()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator TryToSaveToGoogleSheets()
    {
        TrialData earliestData = _storedTrialData.Peek();

        if (earliestData == null || earliestData.all_time == 0)
        {
            yield return new WaitForSeconds(0);
            _storedTrialData.Dequeue();
            if (IsThereUnsavedData())
                StartCoroutine(TryToSaveToGoogleSheets());
            Debug.Log("Null in earliestdata");
        }
        else
        {
            Debug.Log($"alltime: {earliestData.all_time} Sent_text {earliestData.sent_text}");
            using (UnityWebRequest www = UnityWebRequest.Post(TrialData.GetFormURI(), earliestData.GetFormFields()))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                    SaveEverythingToLocalStorage();
                }
                else
                {
                    // Yepp, we will do this one by one
                    _storedTrialData.Dequeue();
                    if (IsThereUnsavedData())
                        StartCoroutine(TryToSaveToGoogleSheets());
                    else
                        ClearLocalStorage();
                }
            }
        }

    }

    private void SaveEverythingToLocalStorage()
    {
        try
        {
            // We store all the data to a disk but do not clear the _storedTrialData. The letter is quite important
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false, System.Text.Encoding.UTF8);
            writer.Write(JsonUtility.ToJson(new SerializableWrapper(_storedTrialData)));
            writer.Close();
            Debug.Log(Application.persistentDataPath + FILE_NAME);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ClearLocalStorage()
    {
        try
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + FILE_NAME, false, System.Text.Encoding.UTF8);
            writer.Close();
            Debug.Log(Application.persistentDataPath + FILE_NAME);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
