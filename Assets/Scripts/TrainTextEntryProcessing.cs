using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrainTextEntryProcessing : MonoBehaviour
{

    public GameObject sentenceField;
    public GameObject confirmButton;
    public TMP_InputField TMP_if;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (string.IsNullOrEmpty(TMP_if.text))
        {
            sentenceField.SetActive(true);
            confirmButton.SetActive(false);
        }
        else
        {
            sentenceField.SetActive(false);
            confirmButton.SetActive(true);
        }
        
    }
}
