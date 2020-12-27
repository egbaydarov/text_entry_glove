using System;
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

    volatile bool ShouldUpdate = false;

    Server server;
    MeasuringMetrics measuringMetrics;
    EntryProcessing entryProcessing;


    public string text { get; private set; }
    private void Awake()
    {
        GameObject objs = GameObject.FindGameObjectWithTag("Server");
        server = objs.GetComponent<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
        entryProcessing = FindObjectOfType<EntryProcessing>();
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
        if (ShouldUpdate)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                    text = text.Capitalize();
                TextField.text = text;
            }
            catch (Exception ex)
            {

            }
            ShouldUpdate = false;
        }
    }


    void UpdateTextFieldAndPredictionsButtons(string data)
    {
        data = data.Trim('\r', '\n');
        string[] data1 = data.Split('#');

        if (data1.Length > 0 && data1[0].Equals("restart"))
            entryProcessing.RestartInput();

        foreach (var i in data1)
            Debug.Log(i);
        string clientMessage = data1.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
        text = clientMessage.Trim('\r', '\n');

#if UNITY_EDITOR
        server.responseDelay.Stop();
        Debug.Log($"RESPONSE DELAY: {server.responseDelay.ElapsedMilliseconds.ToString()}");
#endif
      
        ShouldUpdate = true;
    }

}