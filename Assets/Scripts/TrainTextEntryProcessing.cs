﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

public class TrainTextEntryProcessing : MonoBehaviour
{
    List<string> words = new List<string>();
    Random rnd = new Random();

    public GameObject sentenceField;
    public GameObject confirmButton;
    public TMP_InputField TMP_if;

    public UnityEvent OnTrainEnd;

    int currentSentence;

    int SENTENCE_COUNT = 10;

    void Start()
    {
        for(int i = 0; i < data.Length; ++i)
        {
            if (rnd.Next(data.Length - i) < SENTENCE_COUNT)
                words.Add(data[i]);
        }
    }

    #region sentences
    string[] data = {"Пищевые продукты содержат различные типы углеводов",
"Мы нашли приемлемые решения для обеих сторон",
"Он встряхивал головой и дышал неровно",
"Раньше ненависти также не испытывала",
"Это не будет решением проблемы",
"В первые четыре минуты надо завязать контакт",
"Изредка употребляет в пищу рыбу ",
"Спальник грузится поверх котелка и еды",
"И заслуживают очень строгого обращения ",
"Художественный замысел и его исполнение ",
"Медицинские учреждения страны переполнены пациентами",
"Семья играет в мандалорском обществе важную роль",
"Существенных возвышенностей не имеет",
"Все реже и реже шуршит словарь",
"Потому что строгий анализ нужен редко ",
"Тон дискуссии задали представители церкви",
"Вот так мы восстановили мир в семье",
"Переводили точно и добросовестно",
"И попросить о содействии в этом деле",
"Пропуск мне поставили уже после лекции",
"Только уснуть я могу не раньше пяти часов утра",
"В костюме и шлеме автогонщика в гоночной машине",
"Аналогично этому ведутся дела в других округах ",
"Так экономнее расходуется место",
"Но он должен за активный период заработать на всю жизнь",
"И зимой бы цветы бы расцветали",
"Про мобильные телефоны мне не интересно",
"Хотя и больных в камерах предостаточно",
"Такую возможность можно использовать раз за игру",
"То есть это будет тоже международная компания  ",
"Мы же все сироты бесправные тут ",
"Настоящая наша работа посвящена именно этой проблеме",
"Гений места как будто не привлекал писателя",
"Фактически было несколько таких инцидентов ",
"Я поблагодарил его и спросил его имя ",
"На них тона и фактура отсутствуют",
"Особое внимание следует уделить непродуваемости",
"С балкона моего видно немного эту самую деревню",
"Формирование языка древнегреческой архитектуры",
"Вокруг планеты обращаются две луны",
"Но человека с подходящей энергетикой не оказалось ",
"Формат марки желательно заранее оговорить",
"Поиски долго не давали результатов ",
"Рано или поздно он накажет виновного",
"Эксперты уверены в его победе в первом туре",
"Клинический эффект короткий и ярко выраженный",
"Атмосфера была достаточно спокойной",
"Много заметил веселости и одушевления искреннего",
"При другой системе обучения этого можно было бы избежать",
"Вы не заслужили такого президента",
"Жизненная среда проектируемого объекта ",
"Хорошо ему удавались и эпиграфы ",
"Не выказывает признаков неудовольствия",
"А социальной ответственности мы пока не хотим ",
"Мы с отцом рыбачили в тот момент",
"С чем он справился вполне достойно ",
"Когда это повторение сделается помехой ",
"Это определение не принято и не обговорено",
"Документ будет подписан на высшем уровне  ",
"Этот ритмический рисунок и есть ритм",
"Используется для утверждения государственных документов",
"И еще пять мест во второй десятке ",
"Официального подтверждения это информации пока нет  ",
"Но это чисто наземное глушение ",
"На высокой колокольне храма заливались колокола",
"Только даром порядочных людей беспокоите ",
"При разговоре обращайтесь непосредственно к машинисту",
"Они видели во мне угрозу их контролю",
"Я их воспитывал в очень свободном духе",
"Свободных денег больше не было ",
"На концертах это проявляется в меньшей степени ",
"Прежде всего это касается следующего ",
"Это сейчас очень смешно вспоминать ",
"Особенно следите за шапкой и варежками",
"Национальные черты в архитектуре",
"Популярность изображений городского быта",
"Это уже третье отдельное понятие",
"У страны имелся уже другой вождь и учитель  ",
"Оно само развилось до вполне приличных масштабов",
"Она предпочла тюрьму и изгнание",
"Результат получился неожиданным",
"И уже захватила все лакомые островки",
"Эти признаки устойчивы и характерны",
"Потому что устоявшаяся жизнь так легко не меняется ",
"Такая система просуществовала очень долго ",
"У домашнего телефона накрылся звонок",
"Пока же желаемого единства нет ",
"В течение недели экипажи будут работать совместно",
"Новые герои и темы в искусстве",
"А вот писателем я никогда не хотел быть  ",
"В алтарь они заходить не собирались ",
"Тем более что никто не мешает создать свой личный класс ",
"Тут не пахнет никаким благополучием ",
"В бане был специальный бассейн для контрастных процедур",
"Теперь что касается билетов и визы",
"Он вернулся в избу и стал копаться в углу",
"Все еще уделяется внимание ночным кошмарам",
"Школа была закрыта до понедельника",
"Этот разрыв несколько увеличился с начала августа",
"Такой запрещенный прием меня просто бесит",
"Губернатор смотрел в сторону и молчал",
"Земная кора океанического типа ",
"Дворник указал на дверь налево",
"Начинаем покупать теплую одежду",
"В пределах сквера образуется маленькая площадь",
"Вскоре размеры гробницы были увеличены",
"Пустующее место за обеденным столом",
"А так бы могла получить двойку",
"Студенты изучают технику медитации",
"Гриша был тронут до глубины души",
"До сих пор потоки лавы остаются вязкими и густыми",
"Обычно ученики не рады наказаниям",
"Поразительное разнообразие ландшафтов царит на берегу ",
"он приговаривается к ссылке в каторжные работы",
"Турки отобрали рекорд у китайцев",
"Ведь теперь они воевали за рабство ",
"Он бросил гитару на пол и забрал у нее рацию",
"Глухо отдавались мои шаги в застывающем воздухе",
"Их выступление было по политическим мотивам",
"Исполнителей не может быть меньше двух человек",
"Я отказываюсь в этом участвовать ",
"Уборщица появилась на пороге ровно через неделю",
"Это никогда не было частью великой американской мечты ",
"Избирательно проводится исследование текста песни",
"Они нам теперь нужны как никогда",
"Ни одной бумажки я не выкинула",
"Существуют и другие водные духи ",
"Способную выбирать для своего народа лучшее",
"Определить его границы автор предлагает читателю",
"Первый экзамен проходил в лекционном зале",
"Мы их предлагаем сделать в качестве основных ",
"Редко ходят в гости и редко приглашают к себе",
"Все ищут решение этого вопроса ",
"На этом женская логика подошла к концу",
"Нужно выработать такой механизм",
"При этом все сообщения форума доступны публично ",
"Без парковок и подземных паркингов ",
"Переезжаем к китайской границе",
"Дольше всего он торчал у клеток с тиграми ",
"Англичане же в концовке отыграли только один мяч",
"Не хотелось мне никому плохо делать ",
"Пошли выписали себе ванну и унитаз ",
"Есть несколько заявлений чиновников и строителей ",
"Ведутся мероприятия по возрождению реки",
"У экзаменаторов были большие от удивления глаза ",
"Тем не менее здесь делается масса полезного",
"Не рекомендуются распущенные волосы",
"Можно отвлекаться в любые стороны ",
"О цене устройства пока тоже неизвестно",
"А теперь она снова расползается ",
"Позиции бывают хорошие и плохие",
"Попыток вернуться у нее не было ",
"Свежая струя пробежала по моему лицу ",
"Они оба голодны на любовь и заботу",
"Имя он получил в честь двух дорог",
"О его возвращении ходили анекдоты",
"У девушек были распределены функции ",
"Подобная система принята во многих странах",
"Используется при значительном различии по странам ",
"Вас баловать тоже не приходится ",
"Русские воспринимают синий цвет не так как американцы ",
"Такая странная новая старая вещь",
"Для участия в лекции необходима регистрация",
"Невостребованные животные усыпляются ",
"И все эти планы также были им отвергнуты",
"Этот инструмент совершенствуется  ",
"Используется при значительном различии по странам ",
"Она так и не была восстановлена ",
"Я не знаю лучшего праздника этой эпохи ",
"Эти экспозиции обновляются регулярно",
"Он опять прикорнул перед огнем",
"Эффективность практики подтверждена",
"Для ходьбы предназначена всего треть пространства ",
"У этих двух слизняков нет никакой фантазии",
"Тем не менее проверки продолжаются  ",
"Мы проецируем на них вдохновение",
"К тому времени настоящее имя автора стало известно",
"И всегда пользуйтесь именно ими ",
"Позднее список пополнился живой рыбой",
"А борющийся пролетариат сам поможет себе",
"Такая база встречается чаще всего  ",
"Небо легкое и такое просторное и глубокое ",
"Еще раз сформулируйте пожалуйста ",
"Позже таким же массовым продуктом стал ноутбук ",
"Вы живете и работаете в невероятной свободе",
"Одежда девушек была недопустимой для церкви",
"Полезла в журналы и твиттеры своих студентов",
"В первой записи он пообещал быть откровенным в своем блоге",
"Это самый популярный парк датской столицы ",
"Но только один сумел развить свой талант ",
"Забавно будет на все это посмотреть ",
"Их оборудование не напрямую зависит от станций",
"Практическое руководство по содержанию в неволе ",
"Теперь об остальном снаряжении",
"Но ведь при этом ну совершенно не юзабельные ",
"Использовать существующие программные средства",
"К вечеру он немного успокоился и высказал желание уснуть",
"Это обусловлено следующими факторами",
"А именно сохранить стратегический баланс сил",
"В не особо напряжном темпе и вполне осмысленно",
"Такие фильмы уже практически ушли",
"Это исследование и является предметом моей работы",
"Нас случайным сквозняком согнало",
"Творческую группу формируют из генераторов ",
"Постепенно у нас сложились свои направления",
"И отнюдь не в силу своей замшелости",
"Это мы очень быстро на себе испытали ",
"Сразу после аварии рекомендуется позаботиться о водителе ",
"Рано или поздно расположение камер будет выявлено",
"Но не всегда доверяйте водителю ",
"Устройства могут появиться на рынке осенью ",
"Это снова была любовь с первого взгляда",
"Существуют отрицательно влияющие на все вокруг символы",
"Стрелки показывали без четверти двенадцать ",
"О его личности практически ничего не известно",
"Однако местные власти им об этом напомнили ",
"От первого брака у бизнесмена также двое детей ",
"Боевые подразделения вообще сокращаться не будут",
"В итоге обсерватория превратилась в волшебный парк",
"И тогда все предыдущее умножается на два",
"Телепатией программы пока не обладают",
"Наконец он найден и пиво откупорено",
"Сначала был мальчик из простой крестьянской семьи",
"В своей практике я с таким сталкиваюсь постоянно",
"Возможностей у нашей страны для этого хватает",
"Желания молодых людей не учитывались",
"Также здесь имеет место эффект взаимности",
"Про нежеланных иностранцев тоже не забыли",
"Два отличных курса по безопасности",
"Восстановление самостоятельности и единства страны",
"Принято выпить стопку с духами места",
"Соединение фантазии с реальностью в его работах",
"Однозначного ответа на этот вопрос нет ни у кого",
"Раньше вместо апельсинов в битве использовали яблоки",
"В аудитории послышался протяжный сто"};
    #endregion


    // Update is called once per frame
    void Update()
    {
        sentenceField.GetComponent<Text>().text = words[currentSentence];

        if (string.IsNullOrEmpty(TMP_if.text))
        {
            sentenceField.SetActive(true);
            confirmButton.SetActive(false);
        }
        else
        {
            sentenceField.SetActive(false);
            confirmButton.SetActive(true);
        }
    }

    public void OnNextClicked()
    {
        if(currentSentence + 1 < SENTENCE_COUNT)
        {
            ++currentSentence;

            if (currentSentence == SENTENCE_COUNT - 1)
                confirmButton.GetComponentInChildren<TextMeshProUGUI>().text = "В главное меню. ";
        }
        else
        {
            OnTrainEnd.Invoke();
        }
        //TEMP
        TMP_if.text = "";
    }   
}
