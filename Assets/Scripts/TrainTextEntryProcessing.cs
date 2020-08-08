﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class TrainTextEntryProcessing : MonoBehaviour
{
    List<string> words = new List<string>();
    Random rnd = new Random();



    public GameObject sentenceField;
    public GameObject confirmButton;
    public GameObject menuButton;
    public InputField TMP_if;

    public Text sentenceNumber;
    TextHelper th;
    
    [SerializeField] 
    private InputField intext;

    public UnityEvent OnSentenceEnd;
    public UnityEvent OnTrainEnd;
    public UnityEvent OnMenuClicked;

    Server server;

    GameObject go;
    Shift shift;

    int currentSentence = -1;

    [SerializeField]
    int SENTENCE_COUNT = 8;

    bool isFirstTap = true;

    void Start()
    {
        go = GameObject.Find("keyboard");
        shift = go.GetComponent<Shift>();

        for (int i = 0; i < data.Length; ++i)
        {
            if (rnd.Next(data.Length - i) < SENTENCE_COUNT)
                words.Add(data[i]);
        }
    }
    private void Awake()
    {
        server = FindObjectOfType<Server>();
        th = FindObjectOfType<TextHelper>();
    }

    #region sentences
    string[] data = {"Пусть они так же и остаются сюрпризами",
"Эффективность данного метода чрезвычайно высока",
"Лесник довел лошадь до крыльца и застучал в дверь",
"Пищевые продукты содержат различные типы углеводов",
"Мы нашли приемлемые решения для обеих сторон",
"Он встряхивал головой и дышал неровно",
"Изредка употребляет в пищу рыбу",
"Существенных возвышенностей не имеет",
"Все реже и реже шуршит словарь",
"Потому что строгий анализ нужен редко",
"Тон дискуссии задали представители церкви",
"Ему было жалко этого глупого клоуна",
"Только уснуть я могу не раньше пяти часов утра",
"В костюме и шлеме автогонщика в гоночной машине",
"Аналогично этому ведутся дела в других округах",
"Так экономнее расходуется место",
"Но он должен за активный период заработать на всю жизнь",
"Жизнь армейского офицера известна",
"Сегодня ситуация в армии качественно меняется",
"Результатом станет лучшая пропорция того и другого",
"Встречаются случаи аномально медленного разложения",
"Настоящая наша работа посвящена именно этой проблеме",
"И государство не может быть обязано это сделать",
"Античная демократия и греческое искусство",
"Гений места как будто не привлекал писателя",
"Фактически было несколько таких инцидентов",
"Я поблагодарил его и спросил его имя",
"Женщина в этих местах не может находиться",
"Особое внимание следует уделить непродуваемости",
"Формирование языка древнегреческой архитектуры",
"Пришлось ему оставшееся время работать самому с собой",
"Вокруг планеты обращаются две луны",
"Но человека с подходящей энергетикой не оказалось",
"Быть женой концертирующего пианиста очень непросто",
"Работники предлагают свои знания и умения предпринимателям",
"Поиски долго не давали результатов",
"Эксперты уверены в его победе в первом туре",
"Приглашающая сторона на одну визу одна",
"Клинический эффект короткий и ярко выраженный",
"Много заметил веселости и одушевления искреннего",
"Интернет через систему мобильной связи",
"К ним присоединяются люди со всех близлежащих сел",
"Да и дедлайн установлен десятым марта",
"Хорошо ему удавались и эпиграфы",
"Не выказывает признаков неудовольствия",
"Военный сигнал это практически не задевает",
"Перебарщивают с милыми глупостями",
"Когда это повторение сделается помехой",
"Это определение не принято и не обговорено",
"Этот ритмический рисунок и есть ритм",
"Символические ценности таким образом не измеряются",
"Но это чисто наземное глушение",
"На высокой колокольне храма заливались колокола",
"Только даром порядочных людей беспокоите",
"Они видели во мне угрозу их контролю",
"По многим параметрам ситуация даже стала ухудшаться",
"Я их воспитывал в очень свободном духе",
"Свободных денег больше не было",
"В результате этого шрифт оказался незавершенным",
"На концертах это проявляется в меньшей степени",
"Национальные черты в архитектуре",
"Популярность изображений городского быта",
"И обязательное включение ноутбука все три раза",
"От матери любил убегать к деду с бабкой",
"Премьеру показали в присутствии автора",
"У страны имелся уже другой вождь и учитель",
"Она предпочла тюрьму и изгнание",
"Результат получился неожиданным",
"Я прежде всех про себя расскажу",
"И уже захватила все лакомые островки",
"Эти признаки устойчивы и характерны",
"Потому что устоявшаяся жизнь так легко не меняется",
"Такая система просуществовала очень долго",
"У домашнего телефона накрылся звонок",
"Пока же желаемого единства нет",
"В течение недели экипажи будут работать совместно",
"А вот писателем я никогда не хотел быть",
"Тем более что никто не мешает создать свой личный класс",
"В бане был специальный бассейн для контрастных процедур",
"Теперь что касается билетов и визы",
"Он вернулся в избу и стал копаться в углу",
"Все еще уделяется внимание ночным кошмарам",
"Школа была закрыта до понедельника",
"Разве что по предмету описания и ландшафту",
"Земная кора океанического типа",
"Дворник указал на дверь налево",
"Начинаем покупать теплую одежду",
"Сельское хозяйство остается низкопродуктивным",
"Я сознательно не читал книжку перед просмотром",
"В пределах сквера образуется маленькая площадь",
"Пустующее место за обеденным столом",
"Студенты изучают технику медитации",
"Идеальное воплощение на данный момент",
"Гриша был тронут до глубины души",
"До сих пор потоки лавы остаются вязкими и густыми",
"Обычно ученики не рады наказаниям",
"Поразительное разнообразие ландшафтов царит на берегу",
"он приговаривается к ссылке в каторжные работы",
"Турки отобрали рекорд у китайцев",
"Ведь теперь они воевали за рабство",
"Он бросил гитару на пол и забрал у нее рацию",
"Их выступление было по политическим мотивам",
"Но и здесь никакого направления нет",
"Исполнителей не может быть меньше двух человек",
"Я отказываюсь в этом участвовать",
"Уборщица появилась на пороге ровно через неделю",
"Это никогда не было частью великой американской мечты",
"Ни одной бумажки я не выкинула",
"Способную выбирать для своего народа лучшее",
"Подобное поведение недопустимо",
"Такой согласованности больше нет",
"На этом женская логика подошла к концу",
"Нужно выработать такой механизм",
"При этом все сообщения форума доступны публично",
"Без парковок и подземных паркингов",
"Переезжаем к китайской границе",
"Дольше всего он торчал у клеток с тиграми",
"Англичане же в концовке отыграли только один мяч",
"Именно этого он действительно не говорил",
"Не хотелось мне никому плохо делать",
"Пошли выписали себе ванну и унитаз",
"Процедура наблюдения введена на полгода",
"Есть несколько заявлений чиновников и строителей",
"Ведутся мероприятия по возрождению реки",
"У экзаменаторов были большие от удивления глаза",
"Тем не менее здесь делается масса полезного",
"Можно отвлекаться в любые стороны",
"Должны быть внутренние ограничения",
"А теперь она снова расползается",
"Свежая струя пробежала по моему лицу",
"Они оба голодны на любовь и заботу",
"Имя он получил в честь двух дорог",
"О его возвращении ходили анекдоты",
"Подобная система принята во многих странах",
"Вас баловать тоже не приходится",
"Такая странная новая старая вещь",
"Для участия в лекции необходима регистрация",
"Невостребованные животные усыпляются",
"Этот инструмент совершенствуется",
"Но у самой той обедни приключилось дивное диво",
"Она так и не была восстановлена",
"Я не знаю лучшего праздника этой эпохи",
"Он опять прикорнул перед огнем",
"Паровоз был признан весьма удачным",
"Эффективность практики подтверждена",
"Для ходьбы предназначена всего треть пространства",
"У этих двух слизняков нет никакой фантазии",
"Рассмотрим наши выражения подробнее",
"Поддержка от государства значительно уменьшилась",
"Мы проецируем на них вдохновение",
"К тому времени настоящее имя автора стало известно",
"И всегда пользуйтесь именно ими",
"Небо легкое и такое просторное и глубокое",
"Вы живете и работаете в невероятной свободе",
"Полезла в журналы и твиттеры своих студентов",
"Это самый популярный парк датской столицы",
"Забавно будет на все это посмотреть",
"Не отстают от медиа и скандальные политики",
"Но пока это только предположения",
"Практическое руководство по содержанию в неволе",
"Теперь об остальном снаряжении",
"Но ведь при этом ну совершенно не юзабельные",
"Это обусловлено следующими факторами",
"А именно сохранить стратегический баланс сил",
"Такие фильмы уже практически ушли",
"Нас случайным сквозняком согнало",
"Подвергнута имитации расстрела",
"Творческую группу формируют из генераторов",
"Более смешной фамилии студенты в жизни не слышали",
"Универсальным является метод полного перебора",
"Рано или поздно расположение камер будет выявлено",
"Эти праздники тесно связаны между собой",
"О его личности практически ничего не известно",
"От первого брака у бизнесмена также двое детей",
"Боевые подразделения вообще сокращаться не будут",
"В итоге обсерватория превратилась в волшебный парк",
"И тогда все предыдущее умножается на два",
"Правительству следует ускорить решение этих вопросов",
"Фильтр на малых расстояниях работает плохо",
"Телепатией программы пока не обладают",
"Сначала был мальчик из простой крестьянской семьи",
"Ранее она арендовала серверные мощности",
"Особые приметы не многочисленны",
"В своей практике я с таким сталкиваюсь постоянно",
"Возможностей у нашей страны для этого хватает",
"На него накладывается пух из тростника",
"Также здесь имеет место эффект взаимности",
"Про нежеланных иностранцев тоже не забыли",
"Два отличных курса по безопасности",
"Восстановление самостоятельности и единства страны",
"Принято выпить стопку с духами места",
"Раньше вместо апельсинов в битве использовали яблоки",
"В аудитории послышался протяжный стон",};
    #endregion


    // Update is called once per frame
    void Update()
    {
        if (currentSentence == -1)
            sentenceField.GetComponent<Text>().text = "ТРЕНИРОВКА";
        else
            sentenceField.GetComponent<Text>().text = words[currentSentence];

        sentenceNumber.text = $"Предложение\n{currentSentence + 1}\\{SENTENCE_COUNT}";
    }

    public void OnNextClicked(GameObject obj, PointerEventData pointerData)
    {
        if (obj != null && obj.name.Equals("NextSentence"))
        {
            Shift.ToCapital();
            if (currentSentence + 1 < SENTENCE_COUNT)
            {
                OnSentenceEnd.Invoke();
                ++currentSentence;
                sentenceField.SetActive(true);
                confirmButton.SetActive(false);
                isFirstTap = true;
            }
            else
            {
                OnTrainEnd.Invoke();
                sentenceField.SetActive(false);
                confirmButton.SetActive(false);
                menuButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag == "Key")
        {
            if (isFirstTap)
            {
                Shift.ToSmall();
                server.SendToClient("clear\r\n");
                isFirstTap = false;
                intext.text = "";

                sentenceField.SetActive(false);
                confirmButton.SetActive(true);
            }
        }
        else if (obj != null && obj.tag.Equals("Prediction") && !isFirstTap && !menuButton.activeSelf)
        {
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

    public void OnPointerUpMenu(GameObject obj, PointerEventData pointerData)
    {
        if ((obj != null && obj.name.Equals("StartTrain")))
        {
            server.SendToClient("clear\r\n");
            sentenceField.SetActive(false);
            confirmButton.SetActive(false);
            menuButton.SetActive(false);
            OnMenuClicked.Invoke();
        }
    }
}
