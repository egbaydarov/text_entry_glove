using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class TextHelper : MonoBehaviour
{
    [SerializeField]
    private InputField TextField;
    // Prediction buttons 
    [SerializeField] private Text prediction0;
    [SerializeField] private Text prediction1;
    [SerializeField] private Text prediction2;

    volatile bool ShouldUpdate = false;

    string[] predictions = { "", "", "" };
    int current;

    Server server;

    public static bool isGestureStarted=false;

    public static bool isAllNull;

    public static string text;
    private void Awake()
    {
        GameObject objs = GameObject.FindGameObjectWithTag("Server");
        server = objs.GetComponent<Server>();

        prediction0.resizeTextForBestFit = true;
        prediction1.resizeTextForBestFit = true;
        prediction2.resizeTextForBestFit = true;
    }

    void Start()
    {
        server.OnMessageRecieved.AddListener(UpdateTextFieldAndPredictionsButtons);
    }
    private void OnDisable()
    {
        server.OnMessageRecieved.RemoveListener(UpdateTextFieldAndPredictionsButtons);
    }
    
    private void CleanPredictions()
    {
        prediction0.text = "";
        prediction1.text = "";
        prediction2.text = "";
        
    }

    void Update()
    {
        if (ShouldUpdate)
        {
            TextField.text = text;
            ShouldUpdate = false;
        }

    }

    public void ChangeOnPrediction0()
    {
        if (prediction0.text != "")
        {
            TextField.text =
                $"{TextField.text.Substring(0, TextField.text.Length - predictions[current].Length)}{prediction0.text}";

            current = 0;
        }
    }

    public void ChangeOnPrediction1()
    {
        if (prediction1.text != "")
        {
            TextField.text =
                $"{TextField.text.Substring(0, TextField.text.Length - predictions[current].Length)}{prediction1.text}";

        current = 1;
        }
    }

    public void ChangeOnPrediction2()
    {
        if (prediction2.text != "")
        {
            TextField.text =
                $"{TextField.text.Substring(0, TextField.text.Length - predictions[current].Length)}{prediction2.text}";

            current = 2;
        }
    }

    void UpdateTextFieldAndPredictionsButtons(string data)
    {
        data = data.Trim('\r', '\n');
        string[] data1 = data.Split('#');
        string clientMessage = data1.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
        text = clientMessage.Trim('\r', '\n');
        ShouldUpdate = true;
    }

    public void ClearPredictions()
    {
        prediction0.text = null;
        prediction1.text = null;
        prediction2.text = null;
    }
}