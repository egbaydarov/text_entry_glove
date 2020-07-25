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
    List<string> words;

    public Text tm;
    public Text blockNumber;
    public Text senNumber;
    public InputField TMP_if;
    public GameObject confirmButton;
    public GameObject menuButton;
    public GameObject sentenceField;

    GameObject go;
    Shift shift;

    public UnityEvent OnSentenceInputEnd;
    public UnityEvent OnBlockInputEnd;
    public UnityEvent OnInputEnd;
    public UnityEvent OnMenuClicked;

    #region sentences
    string[] data = {"И это уже дает видимые результаты",
"Встречаются случаи аномально медленного разложения",
"Эффективность данного метода чрезвычайно высока",
"Это не такая уж и короткая тропа",
"Особые приметы не многочисленны",
"Поддержка от государства значительно уменьшилась",
"Военный сигнал это практически не задевает",
"Ему было жалко этого глупого клоуна",
"Разве что по предмету описания и ландшафту",
"Перебарщивают с милыми глупостями",
"Но и здесь никакого направления нет ",
"Жизнь армейского офицера известна ",
"А для меньшинств найдутся другие",
"Трупные явления подразделяются на ранние и поздние",
"Спасибо большое за поздравления ",
"На него накладывается пух из тростника",
"Паровоз был признан весьма удачным",
"Я всегда задавала себе этот вопрос ",
"До десятой вообще мало кто доходит",
"Такой согласованности больше нет",
"Он валит здесь уже второй день",
"Да и дедлайн установлен десятым марта",
"Я прежде всех про себя расскажу",
"Процедура наблюдения введена на полгода ",
"Так что в нынешнем тупике виноваты обе стороны",
"В результате этого шрифт оказался незавершенным ",
"Приглашающая сторона на одну визу одна ",
"Эти праздники тесно связаны между собой ",
"И государство не может быть обязано это сделать",
"Должны быть внутренние ограничения ",
"Более смешной фамилии студенты в жизни не слышали ",
"Часов в десять вечера был доктор",
"Я была готова идти на любую зарплату ",
"Потом материалы будут постепенно добавляться",
"От матери любил убегать к деду с бабкой",
"Но и здесь мы не найдем ответа",
"Занятия философией повели его дальше",
"Мы фактически расплатились с внешними долгами ",
"Подобное поведение недопустимо ",
"Но пока это только предположения ",
"Но у самой той обедни приключилось дивное диво",
"Премьеру показали в присутствии автора",
"Правительству следует ускорить решение этих вопросов ",
"Античная демократия и греческое искусство",
"Интернет через систему мобильной связи",
"И зазвенят в такт колокольчики на шее ",
"И все ваши рассуждения примерно того же уровня",
"Их часто объединяют в одну группу",
"К ним присоединяются люди со всех близлежащих сел",
"Подвергнута имитации расстрела",
"Я сознательно не читал книжку перед просмотром",
"По многим параметрам ситуация даже стала ухудшаться ",
"Лесник довел лошадь до крыльца и застучал в дверь",
"Ранее она арендовала серверные мощности",
"Система требует больших расходов",
"Фильтр на малых расстояниях работает плохо",
"И обязательное включение ноутбука все три раза ",
"Вы не видите что в какой корзине находится",
"Символические ценности таким образом не измеряются",
"Сегодня ситуация в армии качественно меняется ",
"Результатом станет лучшая пропорция того и другого 	",
"Идеальное воплощение на данный момент",
"Универсальным является метод полного перебора",
"С некоторым опозданием он появился",
"Могли бы выиграть и с нами поделиться потом",
"Ты знаешь больше и умеешь лучше",
"Именно этого он действительно не говорил ",
"Сельское хозяйство остается низкопродуктивным ",
"Женщина в этих местах не может находиться",
"Пришлось ему оставшееся время работать самому с собой",
"И не только с технической стороны",
"Они привозили мне только сладкие пирожки",
"Собрания и общественные мероприятия запрещены",
"Рассмотрим наши выражения подробнее",
"Быть женой концертирующего пианиста очень непросто",
"Работники предлагают свои знания и умения предпринимателям",
"Несмотря на оказываемое давление ",
"Пустые машины со свистом проносятся по левым полосам",
"Пусть они так же и остаются сюрпризами",
"Не отстают от медиа и скандальные политики "};
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
        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();
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
        if (obj != null && obj.name.Equals("NextSentence"))
        {

            confirmButton.SetActive(false);
            sentenceField.SetActive(true);

            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                
                OnSentenceInputEnd.Invoke();

                ++currentSentence;
                MeasuringMetrics.SavePrefs();
                

                shift.ToCapital();

                
                full_time.Stop();
                Server.gest_time.Reset();
            }
            else if (currentBlock + 1 < BLOCKS_COUNT)
            {
                
                OnBlockInputEnd.Invoke();
                currentSentence = 0;
                ++currentBlock;
                MeasuringMetrics.SavePrefs();

                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                menuButton.SetActive(true);
            }
            else
            {
                OnInputEnd.Invoke();
            }

            isFirstTap = true;
        }
        else if (isFirstTap && obj != null && obj.tag.Equals("Key"))
        {
            Server.SendToClient("clear\r\n");
            shift.ToSmall();

            isFirstTap = false;

            sentenceField.SetActive(false);
            confirmButton.SetActive(true);
            
            full_time.Restart();
        }
        else if (obj != null && obj.name.Equals("ToMenu"))
        {
            OnMenuClicked.Invoke();
        }
    }

}
