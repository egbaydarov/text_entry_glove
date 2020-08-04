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
        id = (uint) PlayerPrefs.GetInt("Respondent_ID");
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"ID: {id}";
    }

    public void incrementId()
    {
        ++id;
        PlayerPrefs.SetInt("Respondent_ID", (int)id);
    }

    public void decrementId()
    {
        if (id >= 2)
        {
            --id;
            PlayerPrefs.SetInt("Respondent_ID", (int)id);
        }
    }

    private void Awake()
    {
        text = GetComponentInChildren(typeof(Text)) as Text;
    }
}
