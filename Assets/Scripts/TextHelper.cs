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

    public string text { get; private set; }
    private void Awake()
    {
        GameObject objs = GameObject.FindGameObjectWithTag("Server");
        server = objs.GetComponent<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
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
                TextField.text = text.Capitalize();
            }catch(Exception ex)
            {
                
            }
            ShouldUpdate = false;
        }
    }

    
    void UpdateTextFieldAndPredictionsButtons(string data)
    {
        data = data.Trim('\r', '\n');
        string[] data1 = data.Split('#');
        string clientMessage = data1.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
        text = clientMessage.Trim('\r', '\n');


        //конец росчерка
        measuringMetrics.entry_time_sw.Stop();
        measuringMetrics.entry_time += measuringMetrics.search_time_sw.ElapsedMilliseconds;
        measuringMetrics.entry_time_sw.Restart();

        // начало поиска первого
        measuringMetrics.search_time_sw.Restart();

        ShouldUpdate = true;
    }

}