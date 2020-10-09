using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

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

    TextHelper th;
    GameObject go;
    static Shift shift;
    static private Image im;
    
    public static bool isPressed;
    [SerializeField] private InputField intext;
    

    public UnityEvent OnSentenceInputEnd;
    public UnityEvent OnBlockInputEnd;
    public UnityEvent OnInputEnd;
    public UnityEvent OnMenuClicked;


    Server server;

    #region sentences
    string[] data = {"Раньше ненависти также не испытывала",
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
    public UnityEvent disablePinch;

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
        
        // Если нажата кнопка "Ввод завершен" (мб поменять на завершить ввод)
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            //Shift.ToCapital();

           // full_time.Stop();
            //server.gest_time.Stop();
            
            
            isPressed = true;
            confirmButton.SetActive(false);
            sentenceField.SetActive(true);

            // Если в блоке еще есть предложения
            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                
                OnSentenceInputEnd.Invoke();
                Debug.Log("On Sentence End");
                // ResetTime();
                server.SendToClient("clear\r\n");
                ++currentSentence;
                MeasuringMetrics.SavePrefs();
                
                               
                
            }
            // Если в блоке больше нет предложений (это последнее предложение блока)
            else if (currentBlock + 1 < BLOCKS_COUNT)
            {
                
                OnBlockInputEnd.Invoke();
                Debug.Log("On Block End");
                //ResetTime();
                
                currentSentence = 0;
                ++currentBlock;
                MeasuringMetrics.SavePrefs();

                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                disablePinch.Invoke();
                StartCoroutine(Wait());
               // menuButton.SetActive(true);
            }
            // если ввод полностью закончен
            else
            {
                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                OnInputEnd.Invoke();
                ResetTime();
                disablePinch.Invoke();
                StartCoroutine(Wait());
               // menuButton.SetActive(true);
            }
            
            isFirstTap = true;

            //
        }
        // если нажатие на клавиатуру
        else if (isFirstTap && obj != null && obj.tag.Equals("Key"))
        {
            //First PointerDown after OnNextSentence
            if (!menuButton.activeSelf)
            {
                //Shift.ToSmall();
                server.SendToClient("clear\r\n");
                intext.text = "";

                isFirstTap = false;

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag.Equals("Prediction")  && !isFirstTap && !menuButton.activeSelf) 
        {
            MeasuringMetrics.ChoosePredictions();
            switch (obj.name)
            {
                case "Prediction0":
                    th.ChangeOnPrediction0();
                    break;
                case "Prediction1":
                    th.ChangeOnPrediction1();
                    break;
                case "Prediction2":
                    th.ChangeOnPrediction2();
                    break;
                default:
                    break;
            }
        }
        
    }
    
    public void ResetTime()
    {
        //server.gest_time.Reset();
        //server.move_time.Reset();
        //full_time.Reset();
    }

    private void Awake()
    {
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
    }
    public void OnMenuClickedUp(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("ToMenu"))
        {
            OnMenuClicked.Invoke();
        }
    }

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        menuButton.SetActive(true);
    }
}
