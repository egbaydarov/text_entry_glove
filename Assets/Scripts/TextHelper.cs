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

    string[] predictions = { "", "", "" };
    int current;

    Server server;
    volatile bool shouldUpdate;

    public static bool isGestureStarted=false;

    public static bool isAllNull;

    public static string text;
    private void Awake()
    {
        GameObject objs = GameObject.FindGameObjectWithTag("Server");
        server = objs.GetComponent<Server>();
        //server = new Server();
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
        if (shouldUpdate)
        {
            if (TextField.text == "")
            {
                for (int i = 0; i < predictions.Length; ++i)
                    if (predictions[i].Length > 0)
                        predictions[i] = predictions[i].Capitalize();

                TextField.text = predictions[1];
            }
            else
                TextField.text += $" {predictions[1]}";

            prediction0.text = predictions[0];
            prediction1.text = predictions[1];
            prediction2.text = predictions[2];

            shouldUpdate = false;
            current = 1;
        }
        text = TextField.text;
        
        
        if (isGestureStarted)
        {
            CleanPredictions();
            isGestureStarted = false;
        }

        if (prediction0.text == "" && prediction1.text == "" && prediction2.text == "")
        {
            isAllNull = true;
        }
        else
        {
            isAllNull = false;
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
        TextField.text = data;

        //predictions = data.Split(';');

        //for (int i = 0; i < predictions.Length; ++i)
        //    predictions[i] = predictions[i].Trim();

        //if (predictions.Length != 3)
        //{
        //    Debug.Log("Not text data. Skipping ...");
        //}
        //else
        //    shouldUpdate = true;
    }

    public void ClearPredictions()
    {
        prediction0.text = null;
        prediction1.text = null;
        prediction2.text = null;
    }
}