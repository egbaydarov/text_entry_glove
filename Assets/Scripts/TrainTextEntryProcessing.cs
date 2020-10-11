using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class TrainTextEntryProcessing : MonoBehaviour
{
    List<string> words = new List<string>();
    Random rnd = new Random();

    [SerializeField]
    TextAsset sentences;

    public GameObject sentenceField;
    public GameObject confirmButton;
    public GameObject menuButton;
    public InputField TMP_if;

    public Text sentenceNumber;
    TextHelper th;
    
    [SerializeField] 
    private InputField intext;

    public UnityEvent OnSentenceEnd;
    public UnityEvent OnTrainEnd;
    public UnityEvent OnMenuClicked;

    Server server;

    GameObject go;
    Shift shift;

    int currentSentence = -1;

    [SerializeField]
    int SENTENCE_COUNT = 8;

    bool isFirstTap = true;

    void Start()
    {
        if (sentences == null)
        {
            enabled = false;
            Debug.LogError("sentences txt file should be assigned in inspector");
            return;
        }

        data = sentences.text.Split('\n');

        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();

        for (int i = 0; i < data.Length; ++i)
        {
            if (rnd.Next(data.Length - i) < SENTENCE_COUNT)
                words.Add(data[i]);
        }
    }
    private void Awake()
    {
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
    }

    string[] data;


    // Update is called once per frame
    void Update()
    {
        if (currentSentence == -1)
            sentenceField.GetComponent<Text>().text = "ТРЕНИРОВКА";
        else
            sentenceField.GetComponent<Text>().text = words[currentSentence];

        sentenceNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
    }

    public void OnNextClicked(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            //Shift.ToCapital();
            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                OnSentenceEnd.Invoke();
                ++currentSentence;
                sentenceField.SetActive(true);
                confirmButton.SetActive(false);
                isFirstTap = true;
            }
            else
            {
                OnTrainEnd.Invoke();
                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                menuButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag == "Key")
        {
            if (isFirstTap)
            {
                //Shift.ToSmall();
                server.SendToClient("clear\r\n");
                isFirstTap = false;
                intext.text = "";

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag.Equals("Prediction") && !isFirstTap && !menuButton.activeSelf)
        {
            switch (obj.name)
            {
                case "Prediction0":
                    th.ChangeOnPrediction0();
                    break;
                case "Prediction1":
                    th.ChangeOnPrediction1();
                    break;
                case "Prediction2":
                    th.ChangeOnPrediction2();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnPointerUpMenu(GameObject obj, PointerEventData pointerData)
    {
        if ((obj != null && obj.name.Equals("StartTrain")))
        {
            server.SendToClient("clear\r\n");
            sentenceField.SetActive(false);
            confirmButton.SetActive(false);
            menuButton.SetActive(false);
            OnMenuClicked.Invoke();
        }
    }
}
