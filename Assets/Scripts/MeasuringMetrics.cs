using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringMetrics : MonoBehaviour
{

    [SerializeField] private string FORM_URL = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScWlRx70e3SICI3YnnIJSOVPJ3jGoORoAdh-NvsTnuTtVpqkw/formResponse";
    // Start is called before the first frame update
    void Start()
    {
        //WriteMetricsData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Respondent_ID", (int)Settings.id); // Идентификатор испытуемого
        PlayerPrefs.SetString("InputMethod_ID", SceneManagment.method_id); // Идентификатор техники взаимодействия
        PlayerPrefs.SetInt("Attempt_number", EntryProcessing.currentBlock); // Номер блока предложений
        PlayerPrefs.SetInt("Session_number", EntryProcessing.currentSentence); // Номер попытки
        
        Debug.Log("Saved");
    }

    public static void LoadPrefs()
    {
        if (PlayerPrefs.HasKey("Respondent_id"))
        {
            Settings.id = (uint) PlayerPrefs.GetInt("Respondent_id");
            SceneManagment.method_id = PlayerPrefs.GetString("InputMethod_ID");
            EntryProcessing.currentBlock = PlayerPrefs.GetInt("Attempt_number");
            EntryProcessing.currentSentence = PlayerPrefs.GetInt("Session_number");

            Debug.Log(
                $"Loaded : id {Settings.id}, method  {SceneManagment.method_id}, block {EntryProcessing.currentBlock}, sentence {EntryProcessing.currentSentence}");
        }
    }

    public void DeletePrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    IEnumerator Post()
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.952386413", Settings.id.ToString()); // Идентификатор испытуемого
        form.AddField("entry.2024755906", SceneManagment.method_id); // Идентификатор техники взаимодействия
        form.AddField("entry.1905100173",EntryProcessing.currentBlock); // Номер блока предложений
        form.AddField("entry.2130707738",EntryProcessing.currentSentence); // Номер попытки
        form.AddField("entry.1405245047",EntryProcessing.currentSentenceText); // Эталонное предложение
        form.AddField("entry.229951240",Server.mytext); // Введенное испытуемым предложение
        form.AddField("entry.1830134686",Server.mytext.Length); // Длина введенного испытуемым предложения
        form.AddField("entry.1264763496",((float)EntryProcessing.full_time.ElapsedMilliseconds/1000).ToString()); // Время ввода предложения
        form.AddField("entry.452347986","Время перемещения курсора"); // Суммарное время перемещения курсора
        form.AddField("entry.945161006",(((float)Server.gest_time.ElapsedMilliseconds)/1000).ToString()); // Суммарное время вычерчивания росчерка
        form.AddField("entry.2055613067","Время выбора слов"); // Суммарное время выбора слов из списка подсказок
        form.AddField("entry.1730946643",LevenshteinDistance(EntryProcessing.currentSentenceText, Server.mytext)); // Количество неисправленных опечаток   LevenshDistance(EntryProcessing.currentSentenceText, EntryProcessing.currentSentenceText.Length,Server.mytext,Server.mytext.Length).ToString()
        form.AddField("entry.1907294220",(((float)Server.mytext.Length-1)*12.0/EntryProcessing.full_time.Elapsed.Seconds).ToString()); // Скорость набора текста
        
      
        byte[] rawData = form.data;
        WWW www = new WWW(FORM_URL,rawData);
        yield return www;
    }

    public void WriteMetricsData()
    {
        StartCoroutine(Post());
    }
    
    
    public static int LevenshteinDistance(string string1, string string2)
    {
        string1 = string1.ToLower();
        string1 = string1.Replace("ё", "е");
        string2 = string2.ToLower();
        string2 = string2.Replace("ё", "е");
        if (string1 == null) throw new ArgumentNullException("string1");
        if (string2 == null) throw new ArgumentNullException("string2");
        int diff;
        int[,] m = new int[string1.Length + 1, string2.Length + 1];

        for (int i = 0; i <= string1.Length; i++) { m[i, 0] = i; }
        for (int j = 0; j <= string2.Length; j++) { m[0, j] = j; }

        for (int i = 1; i <= string1.Length; i++)
        {
            for (int j = 1; j <= string2.Length; j++)
            {
                diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                        m[i, j - 1] + 1),
                    m[i - 1, j - 1] + diff);
            }
        }
        return m[string1.Length, string2.Length];
    }
    
}
