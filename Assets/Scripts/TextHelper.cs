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
    [SerializeField] private Button button0;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Text prediction0;
    [SerializeField] private Text prediction1;
    [SerializeField] private Text prediction2;

    string[] predictions;

    Server server;
    volatile bool shouldUpdate;

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
        if(shouldUpdate)
        {
            if (TextField.text == "")
            {
                for (var i = 0; i < predictions.Length; i++)
                {
                    predictions[i] = predictions[i].Capitalize();
                }

                TextField.text = predictions[1];
                prediction0.text = predictions[0];
                prediction1.text = predictions[1];
                prediction2.text = predictions[2];
            }
            else
            {

                TextField.text += " " + predictions[1];
                prediction0.text = predictions[0];
                prediction1.text = predictions[1];
                prediction2.text = predictions[2];
            }
            EnableButtons();
            shouldUpdate = false;
        }
    }

    void DisableButtons()
    {
        button0.interactable = false;
        button1.interactable = false;
        button2.interactable = false;
    }

    void EnableButtons()
    {
        button0.interactable = true;
        button1.interactable = true;
        button2.interactable = true;
    }

    public void ChangeOnPrediction0()
    {
        TextField.text = TextField.text.Remove(TextField.text.Length - prediction1.text.Length) + prediction0.text;
        DisableButtons();
    }
    public void ChangeOnPrediction1()
    {
        DisableButtons();
    }
    public void ChangeOnPrediction2()
    {
        TextField.text = TextField.text.Remove(TextField.text.Length - prediction1.text.Length) + prediction2.text;
        DisableButtons();
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