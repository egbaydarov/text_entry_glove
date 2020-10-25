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
    static Shift shift;
    static private Image im;

    public static bool isPressed;
    [SerializeField] private InputField intext;


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

    #region sentences
    string[] data;
    #endregion sentences


    int BLOCKS_COUNT = 8;
    int SENTENCE_COUNT = 8;

    public static int currentBlock;
    public static int currentSentence;
    public static string currentSentenceText;
    public UnityEvent disablePinch;

    public static Stopwatch full_time = new Stopwatch();


    bool isFirstTap = true;

    static public int BackspaceClicked;
    public static int LastSentenceBackspaceClicked { get; set; }

    static public int PredictionClicked;
    public static int LastSentencePredictionClicked { get; set; }

    public static bool IsLastClickPrediction;

    public static int GetBakspaceClicked()
    {
        int res = BackspaceClicked - LastSentenceBackspaceClicked;
        LastSentenceBackspaceClicked = BackspaceClicked;

        return res;
    }

    public static int GetPredictionClicked()
    {
        int res = PredictionClicked - LastSentencePredictionClicked;
        LastSentencePredictionClicked = PredictionClicked;

        return res;
    }


    void Start()
    {
        if (sentences == null)
        {
            enabled = false;
            Debug.LogError("sentences txt file should be assigned in inspector");
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
    }

    // Update is called once per frame
    void Update()
    {
        tm.text =  words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];
        blockNumber.text = $"Блок\n{currentBlock + 1}\\{BLOCKS_COUNT}";
        senNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
        currentSentenceText = words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];
    }

    public void OnNextClicked(GameObject obj, PointerEventData pointerData)
    {
        IsLastClickPrediction = false;
        //UnityEngine.Debug.Log(obj == null ? "null" : $"{obj.name} : {obj.tag}");
        // Если нажата кнопка "Ввод завершен" (мб поменять на завершить ввод)
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            isPressed = true;
            confirmButton.SetActive(false);
            sentenceField.SetActive(true);
            
            // Если в блоке еще есть предложения
            if (currentSentence + 1 < SENTENCE_COUNT)
            {

                OnSentenceInputEnd.Invoke();
                Debug.Log("On Sentence End");
                // ResetTime();
                server.SendToClient("clear\r\n");
                ++currentSentence;
                MeasuringMetrics.SavePrefs();
            }
            // Если в блоке больше нет предложений (это последнее предложение блока)
            else if (currentBlock + 1 < BLOCKS_COUNT)
            {

                OnBlockInputEnd.Invoke();
                Debug.Log("On Block End");
                //ResetTime();

                currentSentence = 0;
                ++currentBlock;
                MeasuringMetrics.SavePrefs();

                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                disablePinch.Invoke();
                StartCoroutine(Wait());
                // menuButton.SetActive(true);
            }
            // если ввод полностью закончен
            else
            {
                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                OnInputEnd.Invoke();
                ResetTime();
                disablePinch.Invoke();
                StartCoroutine(Wait());
                
                
                // menuButton.SetActive(true);
            }
            isFirstTap = true;
        }
        // если первое нажатие на клавиатуру
        else if (isFirstTap && obj != null && obj.tag.Equals("Key"))
        {
            //First PointerDown after OnNextSentence
            if (!menuButton.activeSelf)
            {
                //Shift.ToSmall();
                server.SendToClient("clear\r\n");
                //intext.text = "";

                isFirstTap = false;

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag.Equals("Prediction") && !isFirstTap && !menuButton.activeSelf)
        {
            OnPredictionClicked.Invoke();
            IsLastClickPrediction = true;
            ++PredictionClicked;

        }
        else if (obj != null && obj.tag.Equals("Backspace") && !isFirstTap && !menuButton.activeSelf)
        {
            OnBackspaceClicked.Invoke();
            ++BackspaceClicked;
        }

    }

    public void ResetTime()
    {
    }

    private void Awake()
    {
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
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
