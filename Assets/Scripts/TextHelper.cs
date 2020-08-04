using System.Collections;
using System.Collections.Generic;
using System.Text;
using Leap.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper : MonoBehaviour
{
    [SerializeField]
    private InputField TextField;
    // Prediction buttons 
    [SerializeField] private Text prediction0;
    [SerializeField] private Text prediction1;
    [SerializeField] private Text prediction2;

    string[] predictions = { "", "", "" };

    Server server;
    volatile bool shouldUpdate;
    public bool IsAvailable { get; set; }

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

    void Update()
    {
        if (shouldUpdate)
        {
            if (TextField.text == "")
            {
                for (var i = 0; i < predictions.Length; i++)
                {
                    if (!string.IsNullOrEmpty(predictions[i]))
                        predictions[i] = predictions[i].Capitalize();
                }

                TextField.text = predictions[1];
                prediction0.text = predictions[0];
                prediction1.text = predictions[1];
                prediction2.text = predictions[2].Trim();
                
            }
            else
            {

                TextField.text += " " + predictions[1];
                prediction0.text = predictions[0];
                prediction1.text = predictions[1];
                prediction2.text = predictions[2].Trim();
                
            }
           
            shouldUpdate = false;
            IsAvailable = true;
        }
        text = TextField.text;
    }

    public void ChangeOnPrediction0()
    {
        int index = 0;
        for (int i = TextField.text.Length - 1; i > -1; --i)
        {
            if (TextField.text[i] == ' ')
            {
                index = i;
                break;
            }
        }

        if (index == 0)
            TextField.text = TextField.text.Substring(0, index) + $"{prediction0.text}";
        else
            TextField.text = TextField.text.Substring(0, index) + $" {prediction0.text}";

        IsAvailable = false;
    }

    public void ChangeOnPrediction1()
    {
        int index = 0;
        for (int i = TextField.text.Length - 1; i > -1; --i)
        {
            if (TextField.text[i] == ' ')
            {
                index = i;
                break;
            }
        }

        if (index == 0)
            TextField.text = TextField.text.Substring(0, index) + $"{prediction1.text}";
        else
            TextField.text = TextField.text.Substring(0, index) + $" {prediction1.text}";

        IsAvailable = false;
    }

    public void ChangeOnPrediction2()
    {
        int index = 0;
        for (int i = TextField.text.Length - 1; i > -1; --i)
        {
            if (TextField.text[i] == ' ')
            {
                index = i;
                break;
            }
        }

        if (index == 0)
            TextField.text = TextField.text.Substring(0, index) + $"{prediction2.text}";
        else
            TextField.text = TextField.text.Substring(0, index) + $" {prediction2.text}";
        IsAvailable = false;
    }

    void UpdateTextFieldAndPredictionsButtons(string data)
    {
        predictions = data.Split(';');
        if (predictions.Length != 3)
        {
            Debug.Log("Not text data. Skipping ...");
        }
        else
            shouldUpdate = true;
    }
}