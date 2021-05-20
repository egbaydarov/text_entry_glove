using Leap.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TrialData
{
    public string block_num { get; set; }
    public string sent_num { get; set; }
    public string sent_text { get; set; }
    public string resp_text { get; set; } = "";
    public float all_time { get; set; }
    public string backspace_count { get; set; }
    public string prediction_count { get; set; }
    public string removed_count { get; set; }
    public string type_speed { get; set; }
    public string removing_time { get; set; }
    public string check_time { get; set; }
    public string correction_time { get; set; }
    public string entry_time { get; set; }
    public string search_time { get; set; }
    public string average_distance { get; set; }
    public float yaw_accumulate { get; set; }
    public float pitch_accumulate { get; set; }



    // Instructions were taken from here: https://youtu.be/z9b5aRfrz7M
    private static readonly string _formURI = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSddGyMD2Db2wYOQC-Ix-lbYeeWJfT4t-gxE5TUgs9sYhSo5Sg/formResponse";

    public static string GetFormURI()
    {
        return _formURI;
    }

    public Dictionary<string, string> GetFormFields()
    {
        Dictionary<string, string> form = new Dictionary<string, string>();

        if (sent_text == null)
        {
            Debug.LogWarning("Wrong state of TrialData");
            return form;
        }

        form.Add("entry.1932861945", SwitchABCD.CurrentMode); // Mode
        form.Add("entry.747081656", correction_time.Replace(".", ",")); // Mode
        form.Add("entry.507185938", Settings.id.ToString()); // Идентификатор испытуемого
        form.Add("entry.582005842", SceneManagment.method_id); // Идентификатор техники взаимодействия
        form.Add("entry.347604535", block_num); // Номер блока предложений
        form.Add("entry.1580984151", sent_num); // Номер попытки
        form.Add("entry.832183268", sent_text); // Эталонное предложение
        if (!String.IsNullOrEmpty(resp_text))
            form.Add("entry.1828483782", resp_text.Capitalize()); // Введенное испытуемым предложение
        form.Add("entry.41396143", $"{sent_text.Length}"); // Длина эталонного предложения (символов)
        form.Add("entry.1171184478", $"{sent_text.Count((x) => x == ' ') + 1}"); // Длина эталонного предложения (слов)
        form.Add("entry.2004966619", resp_text == null ? "" : resp_text.Length.ToString()); // Длина введенного испытуемым предложения (символов)
        form.Add("entry.208575183", resp_text == null ? "0" : $"{resp_text.Count((x) => x == ' ') + 1}"); // Длина введенного испытуемым предложения (слов)
        form.Add("entry.202448380", prediction_count);  // сколько раз выбрали подсказку
        form.Add("entry.887164200", removed_count);  // количество удаленных символов
        form.Add("entry.931566926", backspace_count);  // кол-во нажатий backspace
        form.Add("entry.1363907106", LevenshteinDistance(sent_text, resp_text).ToString()); // кол-во неисправленных опечаток
        form.Add("entry.1922697097", all_time.ToString().Replace(".", ",")); // Время ввода предложения
        form.Add("entry.1279543598", search_time.Replace(".", ",")); // Общее время поиска первого символа
        form.Add("entry.938770484", entry_time.Replace(".", ","));  // Общее время ввода росчерка/слова
        form.Add("entry.1875291993", check_time.Replace(".", ",")); // Общее время проверки
        form.Add("entry.647338142", removing_time.Replace(".", ","));  // Общее время удаления слова 
        form.Add("entry.1072504150", average_distance.Replace(".", ","));  // среднее расстояние
        form.Add("entry.1673523306", Math.Round(((float)resp_text.Length) * 12.0 / all_time, 2).ToString().Replace(".", ",")); // Скорость набора текста
        form.Add("entry.1041719792", yaw_accumulate.ToString().Replace(".", ","));
        form.Add("entry.2116604569", pitch_accumulate.ToString().Replace(".", ","));
        //form.Add("entry.1347030375", "");   // Примечание



        return form;
    }

    public static int LevenshteinDistance(string string1, string string2)
    {
        if (string1 == null) throw new ArgumentNullException("string1");
        if (string2 == null || string2.Length == 0)
            return string1.Length;

        string1 = string1.ToLower().Trim();
        string1 = string1.Replace("ё", "е");
        string2 = string2.ToLower().Trim();
        string2 = string2.Replace("ё", "е");


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
