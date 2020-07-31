using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EntryProcessing : MonoBehaviour
{
    public static List<string> words;

    public Text tm;
    public Text blockNumber;
    public Text senNumber;
    public InputField TMP_if;
    public GameObject confirmButton;
    public GameObject menuButton;
    public GameObject sentenceField;

    
    GameObject go;
    static Shift shift;
    static private Image im;
    
    public static bool isPressed;

    public UnityEvent OnSentenceInputEnd;
    public UnityEvent OnBlockInputEnd;
    public UnityEvent OnInputEnd;
    public UnityEvent OnMenuClicked;

    #region sentences
    string[] data = {"﻿Раньше ненависти также не испытывала",
"Формат марки желательно заранее оговорить",
"Постепенно у нас сложились свои направления",
"В алтарь они заходить не собирались",
"Хотя и больных в камерах предостаточно",
"То есть это будет тоже международная компания",
"Избирательно проводится исследование текста песни",
"Переводили точно и добросовестно",
"Позднее список пополнился живой рыбой",
"Губернатор смотрел в сторону и молчал",
"Их оборудование не напрямую зависит от станций",
"Я была готова идти на любую зарплату",
"Про мобильные телефоны мне не интересно",
"Прежде всего это касается следующего",
"Вскоре размеры гробницы были увеличены",
"Устройства могут появиться на рынке осенью",
"Такой запрещенный прием меня просто бесит",
"А социальной ответственности мы пока не хотим",
"Этот разрыв несколько увеличился с начала августа",
"Художественный замысел и его исполнение",
"К вечеру он немного успокоился и высказал желание уснуть",
"Они нам теперь нужны как никогда",
"Вот так мы восстановили мир в семье",
"Потом материалы будут постепенно добавляться",
"Часов в десять вечера был доктор",
"Позиции бывают хорошие и плохие",
"Занятия философией повели его дальше",
"И зимой бы цветы бы расцветали",
"У девушек были распределены функции",
"Существуют отрицательно влияющие на все вокруг символы",
"С балкона моего видно немного эту самую деревню",
"И попросить о содействии в этом деле",
"Используется при значительном различии по странам",
"Их часто объединяют в одну группу",
"Это исследование и является предметом моей работы",
"Так что в нынешнем тупике виноваты обе стороны",
"Он валит здесь уже второй день",
"Но только один сумел развить свой талант",
"На них тона и фактура отсутствуют",
"Такую возможность можно использовать раз за игру",
"Редко ходят в гости и редко приглашают к себе",
"Документ будет подписан на высшем уровне",
"Это не будет решением проблемы",
"Ты знаешь больше и умеешь лучше",
"При другой системе обучения этого можно было бы избежать",
"Это снова была любовь с первого взгляда",
"Мы их предлагаем сделать в качестве основных",
"Такую возможность можно использовать раз за игру",
"С балкона моего видно немного эту самую деревню",
"Это уже третье отдельное понятие",
"Собрания и общественные мероприятия запрещены",
"Медицинские учреждения страны переполнены пациентами",
"Однако местные власти им об этом напомнили",
"Тем не менее проверки продолжаются",
"И это уже дает видимые результаты",
"Такая база встречается чаще всего",
"Система требует больших расходов",
"В первые четыре минуты надо завязать контакт",
"С некоторым опозданием он появился",
"С чем он справился вполне достойно",
"И не только с технической стороны",
"Вы не заслужили такого президента",
"Русские воспринимают синий цвет не так как американцы",
"Атмосфера была достаточно спокойной"};
    #endregion sentences


    int BLOCKS_COUNT = 8;
    int SENTENCE_COUNT = 8;

    public static int currentBlock;
    public static int currentSentence;
    public static string currentSentenceText;

    public static Stopwatch full_time = new Stopwatch();
    

    bool isFirstTap = true;

    void Start()
    {
        words = new List<string>(data);
    }

    // Update is called once per frame
    void Update()
    {
        tm.text = words[SENTENCE_COUNT * currentBlock + currentSentence];
        blockNumber.text = $"Блок\n{currentBlock + 1}\\{BLOCKS_COUNT}";
        senNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
        currentSentenceText = words[SENTENCE_COUNT * currentBlock + currentSentence];
    }

    public void OnNextClicked(GameObject obj, PointerEventData pointerData)
    {
        //UnityEngine.Debug.Log(obj == null ? "null" : $"{obj.name} : {obj.tag}");
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            Shift.ToCapital();

            full_time.Stop();
            Server.gest_time.Stop();
            
            
            isPressed = true;
            confirmButton.SetActive(false);
            sentenceField.SetActive(true);

            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                
                OnSentenceInputEnd.Invoke();
                ResetTime();
                
                ++currentSentence;
                MeasuringMetrics.SavePrefs();
                               
                
            }
            else if (currentBlock + 1 < BLOCKS_COUNT)
            {
                
                OnBlockInputEnd.Invoke();
                ResetTime();
                
                currentSentence = 0;
                ++currentBlock;
                MeasuringMetrics.SavePrefs();

                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                menuButton.SetActive(true);
                OnInputEnd.Invoke();
                ResetTime();
            }

            isFirstTap = true;
        }
        else if (isFirstTap && obj != null && obj.tag.Equals("Key") && !menuButton.activeSelf)
        {
            Shift.ToSmall();
            Server.mytext = "";
            
            
            isFirstTap = false;

            sentenceField.SetActive(false);
            confirmButton.SetActive(true);
            
            
        }
        
    }
    
    public static void ResetTime()
    {
        Server.gest_time.Reset();
        Server.move_time.Reset();
        full_time.Reset();
    }


    public void OnMenuClickedUp(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("ToMenu"))
        {
            Server.SendToClient("clear\r\n");
            OnMenuClicked.Invoke();
        }
    }
}
