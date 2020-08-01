using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Text text;

    public static uint id;

    void Start()
    {
        if(!PlayerPrefs.HasKey("Respondent_ID"))
            id = 1;
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
        if (id >= 1)
        {
            --id;
        }
    }

    private void Awake()
    {
        text = GetComponentInChildren(typeof(Text)) as Text;
    }
}
