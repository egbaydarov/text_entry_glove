using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTrainWords : MonoBehaviour
{
    List<string> words;

    public Text tm;
    public TMP_InputField TMP_if;
    public GameObject confirmButton;
    public GameObject sentenceField;

    string[] data = {"abd","abv"};

    int current;

    void Start()
    {
        words = new List<string>(data);
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = words[current];

        if (String.IsNullOrEmpty(TMP_if.text))
        {
            confirmButton.SetActive(false);
            sentenceField.SetActive(true);
        }
        else
        {
            confirmButton.SetActive(true);
            sentenceField.SetActive(false);
        }
    }

    public void inc_current()
    {
        if (current + 1 > words.Count - 1)
            current = 0;
        else
            ++current;
    }

    public void dec_current()
    {
        if (current - 1 < 0)
            current = words.Count - 1;
        else
            --current;
    }
}
