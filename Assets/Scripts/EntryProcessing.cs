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

    [SerializeField]
    GameObject icons;

    public string LastTagDown { get; private set; } = "";
    bool isFirstSingleKeyDown { get; set; }

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
    public int currentSentence { get; set; }
    public string currentSentenceText;
    public UnityEvent disablePinch;

    MeasuringMetrics measuringMetrics;

    Stopwatch BackspaceDownTime = new Stopwatch();

    bool BackspacePressed { get; set; } = false;
    bool ShoudSetToStart { get; set; } = false;

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

        words = new List<string>(data);

        AssignListners();
    }

    public void regenerateSentences()
    {
        Debug.Log("Create new sentences order");
        SentenceOrder = new int[data.Length];

        for (int i = 0; i < data.Length; ++i)
            SentenceOrder[i] = i;

        SentenceOrder = SentenceOrder.OrderBy(x => rnd.Next()).ToArray();
        for (int i = 0; i < data.Length; ++i)
        {
            PlayerPrefs.SetInt($"SentenceOrder{i}", SentenceOrder[i]);
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if(!SceneManagment.isMain)
        {
            tm.text = words[SentenceOrder[currentSentence + 64]];
            currentSentenceText = words[SentenceOrder[currentSentence + 64]];
            return;
        }

        tm.text = words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];
        blockNumber.text = $"Блок\n{currentBlock + 1}\\{BLOCKS_COUNT}";
        senNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
        currentSentenceText = words[SentenceOrder[SENTENCE_COUNT * currentBlock + currentSentence]];

        
        if (ShoudSetToStart)
            setToStart();
    }

    void AssignListners()
    {
        OnSentenceInputEnd.AddListener(() =>
        {
            Debug.Log("OnSentenceInputEnd: INVOKED");
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

    public void setToStart()
    {
        icons.SetActive(true);

        confirmButton.SetActive(false);

        sentenceField.SetActive(true);

        ShoudSetToStart = false;
    }

    public void RestartInput()
    {
        server.SendToClient("clear\r\n");

        ShoudSetToStart = true;
    }

    public void OnNextDown(GameObject obj, PointerEventData pointerData)
    {

        //UnityEngine.Debug.Log(obj == null ? "null" : $"{obj.name} : {obj.tag}");
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            icons.SetActive(true);

            LastTagDown = "NextSentence";

            measuringMetrics.EndSentenceInput();

            confirmButton.SetActive(false);
            sentenceField.SetActive(true);

            if(!SceneManagment.isMain)
            {
                OnSentenceInputEnd.Invoke();
                return;
            }

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

            measuringMetrics.ChoosePrediction();

            OnPredictionClicked.Invoke();
            isFirstSingleKeyDown = true;
        }
    }

    public void OnBackspaceDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Backspace") && !menuButton.activeSelf)
        {
            BackspacePressed = true;
            LastTagDown = "Backspace";
            BackspaceDownTime.Restart();

            //начало нажатия на backspace
            //measuringMetrics.remove_time_sw.Restart();

            OnBackspaceClicked.Invoke();

            server.SendToClient("backspace\r\n");

            isFirstSingleKeyDown = true;
        }
    }

    public void OnBackspaceUp(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Backspace") && !menuButton.activeSelf)
        {
            BackspacePressed = false;

            measuringMetrics.DeleteWord();

            isFirstSingleKeyDown = true;
        }
    }


    public void OnKeyboardDown(GameObject obj, PointerEventData pointerData)
    {

        //check valid
        if ((obj != null && obj.tag.Equals("Key") && !menuButton.activeSelf) |
            (obj != null && obj.tag.Equals("Key") && !SceneManagment.isMain))
        {
            LastTagDown = "Key";

            //Первое нажатие после заучивания предложения
            if (!confirmButton.activeSelf)
            {
                if (!String.IsNullOrEmpty(th.text))
                    server.SendToClient("clear\r\n");


                measuringMetrics.sent_text = (string)currentSentenceText.Clone();

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
                measuringMetrics.StartSentenceInput();

            }

            StartCoroutine(WaitForSec());
            Debug.Log("ICONS OFF");
        }
    }


    public void OnSpaceDown(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("Space") && !menuButton.activeSelf)
        {
            isFirstSingleKeyDown = true;
        }
    }

    private void Awake()
    {
        //  icons.SetActive(true);
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
    }
    public void OnMenuClickedUp(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("ToMenu"))
        {
            server.SendToClient("clear\r\n");
            OnMenuClicked.Invoke();
            currentSentence = 0;
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        menuButton.SetActive(true);
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds((float)0.5);
        icons.SetActive(false);
    }


    public void OnPointerEnter(GameObject obj, PointerEventData pointerData)
    {
        if (obj.name.Equals("CanvasInputField") || obj.name.Equals("NextSentence") || obj.CompareTag("Prediction"))
        {
            
        }
    }

    public void OnPointerExit(GameObject obj, PointerEventData pointerData)
    {
        //Debug.Log($"HIGHLIGHT TAG {obj.tag} NAME: {obj.name}");
        if (obj.name.Equals("CanvasInputField") || obj.name.Equals("NextSentence") || obj.CompareTag("Prediction"))
        {
           
        }
    }

}
