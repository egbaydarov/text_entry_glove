using System;
using System.Collections.Generic;
using System.Linq;
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
    public string resp_text = "";
    
    // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
    private static readonly string _formURI = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddGyMD2Db2wYOQC-Ix-lbYeeWJfT4t-gxE5TUgs9sYhSo5Sg/formResponse";

    public static string GetFormURI()
    {
        return _formURI;
    }

    public Dictionary<string, string> GetFormFields()
    {
        if (TextHelper.text != null && TextHelper.text.Length > 0)
        {
            if (TextHelper.text[TextHelper.text.Length - 1] == ' ')
                TextHelper.text = TextHelper.text.Remove(TextHelper.text.Length - 1);
        }
        Dictionary<string, string> form = new Dictionary<string, string>();
        
        form.Add("entry.507185938", Settings.id.ToString()); // Идентификатор испытуемого
        form.Add("entry.582005842", SceneManagment.method_id); // Идентификатор техники взаимодействия
        form.Add("entry.347604535", (block_num+1).ToString()); // Номер блока предложений
        form.Add("entry.1580984151", (sent_num+1).ToString()); // Номер попытки
        form.Add("entry.832183268", sent_text); // Эталонное предложение
        form.Add("entry.1828483782", resp_text); // Введенное испытуемым предложение
        form.Add("entry.41396143", $"{sent_text.Length}:{sent_text.Count((x) => x == ' ') + 1}"); // Длина эталонного предложения
        form.Add("entry.2004966619", $"{resp_text.Length}:{sent_text.Count((x) => x == ' ') + 1}"); // Длина введенного испытуемым предложения
        form.Add("entry.202448380", "22"); // сколько раз выбрали подсказку
        form.Add("entry.887164200", "33"); // количество удаленных символов
        form.Add("entry.931566926", "44"); // кол-во нажатий backspace
        form.Add("entry.1363907106", "55"); // кол-во исправленных опечаток
        form.Add("entry.1922697097", all_time.ToString().Replace(".",",")); // Время ввода предложения
        form.Add("entry.1279543598", "66"); // Общее время поиска первого символа
        form.Add("entry.938770484", "77"); // Общее время ввода росчерка/слова
        form.Add("entry.1875291993", "88"); // Общее время проверки и коррекции
        form.Add("entry.647338142", "99"); // Общее время удаления слова 
        form.Add("entry.1673523306", Math.Round(((float) resp_text.Length) * 12.0 / all_time, 2).ToString().Replace(".",",")); // Скорость набора текста
        form.Add("entry.1347030375", ""); // Примечание
                
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
