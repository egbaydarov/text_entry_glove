using System;
using System.Collections.Generic;
using System.Linq;
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


    public UnityEvent OnSentenceInputEnd;

    public string LastTagDown { get; private set; } = "";
    int[] SentenceOrder;


    Server server;

    GameObject go;

    int currentSentence = -1;


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

        SentenceOrder = new int[data.Length];

        for (int i = 0; i < data.Length; ++i)
            SentenceOrder[i] = i;

        Debug.Log("Create new sentences order");
        SentenceOrder = SentenceOrder.OrderBy(x => rnd.Next()).ToArray();
        for (int i = 0; i < data.Length; ++i)
        {
            PlayerPrefs.SetInt($"SentenceOrder{i}", SentenceOrder[i]);
        }
        PlayerPrefs.Save();

        words = new List<string>(data);

    }


    string[] data;


    // Update is called once per frame
    void Update()
    {
        if (currentSentence == -1)
            sentenceField.GetComponent<Text>().text = "ТРЕНИРОВКА";
        else
            sentenceField.GetComponent<Text>().text = words[SentenceOrder[currentSentence + 64]];

    }


    public void OnMenuClickedUp(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("ToMenu"))
        {
            server.SendToClient("clear\r\n");
        }
    }

    private void Awake()
    {
        //  icons.SetActive(true);
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
    }



    [SerializeField]
    GameObject icons;

    public void OnNextDown(GameObject obj, PointerEventData pointerData)
    {

        //UnityEngine.Debug.Log(obj == null ? "null" : $"{obj.name} : {obj.tag}");
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            icons.SetActive(true);
            ++currentSentence;

            LastTagDown = "NextSentence";

            OnSentenceInputEnd.Invoke();
            confirmButton.SetActive(false);
            sentenceField.SetActive(true);
        }
    }

    public void OnPredictionDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Prediction") && !menuButton.activeSelf)
        {
            LastTagDown = "Prediction";
        }
    }

    public void OnBackspaceDown(GameObject obj, PointerEventData pointerData)
    {
        //check valid
        if (obj != null && obj.tag.Equals("Backspace") && !menuButton.activeSelf)
        {
            LastTagDown = "Backspace";

            //начало нажатия на backspace
            //measuringMetrics.remove_time_sw.Restart();
            server.SendToClient("backspace\r\n");
        }
    }

    public void OnKeyboardDown(GameObject obj, PointerEventData pointerData)
    {

        //check valid
        if (obj != null && obj.tag.Equals("Key") && !menuButton.activeSelf)
        {
            LastTagDown = "Key";

            //Первое нажатие после заучивания предложения
            if (!confirmButton.activeSelf)
            {
                if (!String.IsNullOrEmpty(th.text))
                    server.SendToClient("clear\r\n");

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }

            Debug.Log("ICONS OFF");
        }
    }
}
