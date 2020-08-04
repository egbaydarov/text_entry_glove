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
            prediction0.fontSize = 40 + 20 / predictions[0].Length;
            prediction1.fontSize = 40 + 20 / predictions[1].Length;
            prediction2.fontSize = 40 + 20 / predictions[2].Length;
            
            shouldUpdate = false;
            IsAvailable = true;
        }
        text = TextField.text;
    }

    public void ChangeOnPrediction0()
    {
        TextField.text = TextField.text.Remove(TextField.text.Length - prediction1.text.Length) + prediction0.text;
        IsAvailable = false;
    }
    public void ChangeOnPrediction1()
    {
        IsAvailable = false;
    }
    public void ChangeOnPrediction2()
    {
        TextField.text = TextField.text.Remove(TextField.text.Length - prediction1.text.Length) + prediction2.text;
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