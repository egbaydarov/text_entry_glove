﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper: MonoBehaviour
{
    [SerializeField]
    private TMP_InputField intext;
    // Prediction buttons 

    private void Awake()
    {
        //server = go.GetComponent<Server>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // parse mytext in array, middle word goes to intext 
        if (!string.IsNullOrEmpty(Server.mytext))
        {
            intext.text = Server.mytext;
            Server.mytext = "";
        }
        
    }
}