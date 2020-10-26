using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class EntryProcessing : MonoBehaviour
{
    public static List<string> words;

    static System.Random rnd = new System.Random();

    public Text tm;
    public Text blockNumber;
    public Text senNumber;
    public InputField TMP_if;
    public GameObject confirmButton;
    public GameObject menuButton;
    public GameObject sentenceField;

    TextHelper th;
    GameObject go;

    [SerializeField]
    private InputField intext;
    
    public string LastTagDown { get; private set; }

    public UnityEvent OnSentenceInputEnd;
    public UnityEvent OnBlockInputEnd;
    public UnityEvent OnInputEnd;
    public UnityEvent OnMenuClicked;
    public UnityEvent OnPredictionClicked;
    public UnityEvent OnBackspaceClicked;

    [SerializeField]
    TextAsset sentences;

    int[] SentenceOrder;

    Server server;
    string[] data;


    int BLOCKS_COUNT = 8;
    int SENTENCE_COUNT = 8;

    public int currentBlock;
    public int currentSentence;
    public string currentSentenceText;
    public UnityEvent disablePinch;

    MeasuringMetrics measuringMetrics;

    //TODO Change storing to serialization
    void Start()
    {
        if (sentences == null)
        {
            enabled = false;
            Debug.LogError("sentences.txt file should be assigned in inspector");
            return;
        }

        data = sentences.text.Split('\n');

        SentenceOrder = new int[data.Length];

        for (int i = 0; i < data.Length; ++i)
            SentenceOrder[i] = i;

        if (PlayerPrefs.HasKey("SentenceOrder0"))
        {
            Debug.Log("Load sentences from saved");
            for (int i = 0; i < data.Length; ++i)
            {
                SentenceOrder[i] = PlayerPrefs.GetInt($"SentenceOrder{i}");
            }
        }
        else
        {
            Debug.Log("Create new sentences order");
            SentenceOrder = SentenceOrder.OrderBy(x => rnd.Next()).ToArray();
            for (int i = 0; i < data.Length; ++i)
            {
                PlayerPrefs.SetInt($"SentenceOrder{i}", SentenceOrder[i]);
            }
            PlayerPrefs.Save();
        }

        words = new List<string>(data);

        AssignListners();
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];
        blockNumber.text = $"Блок\n{currentBlock + 1}\\{BLOCKS_COUNT}";
        senNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
        currentSentenceText = words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];
    }

    void AssignListners()
    {
        OnSentenceInputEnd.AddListener(() =>
        {
            Debug.Log("OnSentenceInputEnd: INVOKED");
            server.SendToClient("clear\r\n");

            if (currentSentence + 1 < SENTENCE_COUNT)
                ++currentSentence;
        });

        OnBlockInputEnd.AddListener(() =>
        {
            Debug.Log("OnBlockInputEnd: INVOKED");
            currentSentence = 0;

            if (currentBlock + 1 < BLOCKS_COUNT)
                ++currentBlock;

            sentenceField.SetActive(false);
            confirmButton.SetActive(false);
            disablePinch.Invoke();
            StartCoroutine(Wait());
        });

        OnInputEnd.AddListener(() =>
        {
            Debug.Log("OnInputEnd: INVOKED");
            currentBlock = 0;
        });
    }


    public void OnNextDown(GameObject obj, PointerEventData pointerData)
    {
        //UnityEngine.Debug.Log(obj == null ? "null" : $"{obj.name} : {obj.tag}");
        LastTagDown = "null";
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            LastTagDown = "NextSentence";

            confirmButton.SetActive(false);
            sentenceField.SetActive(true);

            // Если в блоке еще есть предложения
            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                OnSentenceInputEnd.Invoke();
            }
            // Если в блоке больше нет предложений (это последнее предложение блока)
            else if (currentBlock + 1 < BLOCKS_COUNT)
            {
                OnSentenceInputEnd.Invoke();
                OnBlockInputEnd.Invoke();
            }
            // если ввод полностью закончен
            else
            {
                OnSentenceInputEnd.Invoke();
                OnBlockInputEnd.Invoke();
                OnInputEnd.Invoke();
            }
        }
    }

    public void OnPredictionDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Prediction") && !menuButton.activeSelf)
        {
            LastTagDown = "Prediction";

            //счетчик нажатий на подсказку
            ++measuringMetrics.prediction_choose;

            //начало поиска первого
            measuringMetrics.search_time_sw.Restart();
            
            //конец росчерка
            measuringMetrics.entry_time_sw.Stop();
            measuringMetrics.entry_time += measuringMetrics.search_time_sw.ElapsedMilliseconds;
            measuringMetrics.entry_time_sw.Reset();

            OnPredictionClicked.Invoke();
        }
    }

    public void OnBackspaceDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Backspace") && !menuButton.activeSelf)
        {
            LastTagDown = "Backspace";

            //счетчик нажатий на подсказку
            ++measuringMetrics.backspace_choose;

            // начало поиска первого
            measuringMetrics.search_time_sw.Restart();

            //конец росчерка
            measuringMetrics.entry_time_sw.Stop();
            measuringMetrics.entry_time += measuringMetrics.search_time_sw.ElapsedMilliseconds;
            measuringMetrics.entry_time_sw.Reset();

            //начало нажатия на backspace
            measuringMetrics.remove_time_sw.Start();

            OnBackspaceClicked.Invoke();
        }
    }

    public void OnBackspaceUp(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Backspace") && !menuButton.activeSelf)
        {
            //конец нажатия на backspace
            measuringMetrics.remove_time_sw.Stop();
            measuringMetrics.remove_time += measuringMetrics.remove_time_sw.ElapsedMilliseconds;
            measuringMetrics.remove_time_sw.Reset();
        }
    }


    public void OnKeyboardDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Key"))
        {
            LastTagDown = "Key";

            measuringMetrics.search_time_sw.Stop();
            measuringMetrics.search_time += measuringMetrics.search_time_sw.ElapsedMilliseconds;
            measuringMetrics.search_time_sw.Reset();

            measuringMetrics.entry_time_sw.Restart();

            //Первое нажатие после заучивания предложения
            if (!menuButton.activeSelf)
            {
                server.SendToClient("clear\r\n");

                measuringMetrics.EndSentenceInput();

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }
        }
    }

    private void Awake()
    {
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
    }
    public void OnMenuClickedUp(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("ToMenu"))
        {
            OnMenuClicked.Invoke();
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        menuButton.SetActive(true);
    }

}
