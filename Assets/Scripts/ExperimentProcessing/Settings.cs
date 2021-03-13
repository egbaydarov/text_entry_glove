using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Text text;

    [SerializeField] private Text CurrentPointingEye;

    public static bool isRightEye { get; set; }

    public void setRightEye(bool value)
    {
        isRightEye = value;

        if (value)
            PlayerPrefs.SetString("PointEye", "Right");
        else
            PlayerPrefs.SetString("PointEye", "Left");
    }

    public static uint id;

    void Start()
    {
        id = (uint)PlayerPrefs.GetInt("Respondent_ID");

        if (PlayerPrefs.GetString("PointEye") != null && PlayerPrefs.GetString("PointEye").Equals("Right"))
        {
            isRightEye = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"ID: {id}";

        CurrentPointingEye.text = isRightEye ? "Правый глаз" : "Левый глаз";
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
