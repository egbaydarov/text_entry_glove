using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class TrialData
{
    public int block_num;
    public int sent_num;
    public string sent_text;
    public float all_time;
    public float gest_time;
    public float move_time;
    public float choose_time;
    public float fix_choose_time;
    public float wait_time;
    
    
    
    
    

    // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
    private static readonly string _formURI = "https://docs.google.com/forms/u/0/d/e/1FAIpQLScWlRx70e3SICI3YnnIJSOVPJ3jGoORoAdh-NvsTnuTtVpqkw/formResponse";

    public static string GetFormURI()
    {
        return _formURI;
    }

    public Dictionary<string, string> GetFormFields()
    {
        if (TextHelper.text.Length > 0)
        {
            if (TextHelper.text[TextHelper.text.Length - 1] == ' ')
                TextHelper.text = TextHelper.text.Remove(TextHelper.text.Length - 1);
        }
        Dictionary<string, string> form = new Dictionary<string, string>();
        
        form.Add("entry.952386413", Settings.id.ToString()); // Идентификатор испытуемого
        form.Add("entry.2024755906", SceneManagment.method_id); // Идентификатор техники взаимодействия
        form.Add("entry.1905100173", (block_num+1).ToString()); // Номер блока предложений
        form.Add("entry.2130707738", (sent_num+1).ToString()); // Номер попытки
        form.Add("entry.1405245047", sent_text); // Эталонное предложение
        form.Add("entry.229951240", TextHelper.text); // Введенное испытуемым предложение
        form.Add("entry.1830134686", TextHelper.text.Length.ToString()); // Длина введенного испытуемым предложения
        form.Add("entry.1264763496", all_time.ToString().Replace(".",",")); // Время ввода предложения
        form.Add("entry.452347986", move_time.ToString().Replace(".",",")); // Суммарное время перемещения курсора
        form.Add("entry.945161006", gest_time.ToString().Replace(".",",")); // Суммарное время вычерчивания росчерка
        form.Add("entry.2055613067", choose_time.ToString().Replace(".",",")); // Суммарное время выбора слов из списка подсказок
        form.Add("entry.824354990", fix_choose_time.ToString().Replace(".",",")); // Скорректированное ремя выбора слов
        form.Add("entry.254841772", wait_time.ToString().Replace(".",",")); //Время ожидания подсказок
        form.Add("entry.1730946643", LevenshteinDistance(sent_text, TextHelper.text).ToString()); // Количество неисправленных опечаток  
        form.Add("entry.1907294220", Math.Round(((float) TextHelper.text.Length) * 12.0 / all_time, 2).ToString().Replace(".",",")); // Скорость набора текста
        
        
        
        
        return form;
    }
    
    public static int LevenshteinDistance(string string1, string string2)
    {
        string1 = string1.ToLower();
        string1 = string1.Replace("ё", "е");
        string2 = string2.ToLower();
        string2 = string2.Replace("ё", "е");
        //Debug.Log(string1);
        //Debug.Log(string2);
        
       // for(int i =0;i<string1.Length;i++)
            //Debug.Log(string1[i].Equals(string2[i]));
        //if (string2.Length > 0)
        //string2.Remove(string2.Length - 1);
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
