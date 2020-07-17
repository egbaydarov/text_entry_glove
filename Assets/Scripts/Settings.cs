﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    Text text;

    uint id;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"ID: {id}";
    }

    public void incrementId()
    {
        ++id;
    }

    public void decrementId()
    {
        --id;
    }

    private void Awake()
    {
        text = GetComponentInChildren(typeof(Text)) as Text;
    }
}