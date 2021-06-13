using Leap.Unity;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class TextHelper : MonoBehaviour
{
    [SerializeField]
    private InputField TextField;

    volatile bool ShouldUpdate = false;

    Server server;
    SwitchABCD switchABCD;
    MeasuringMetrics measuringMetrics;
    EntryProcessing entryProcessing;

    public string text { get; private set; }
    private void Awake()
    {
        GameObject objs = GameObject.FindGameObjectWithTag("Server");
        server = objs.GetComponent<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
        entryProcessing = FindObjectOfType<EntryProcessing>();
        switchABCD = FindObjectOfType<SwitchABCD>();
    }

    void Start()
    {
        server.OnMessageRecieved.AddListener(UpdateTextFieldAndPredictionsButtons);
    }
    private void OnDisable()
    {
        server.OnMessageRecieved.RemoveListener(UpdateTextFieldAndPredictionsButtons);

    }


    void Update()
    {
        if (ShouldUpdate)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                    text = text.Capitalize();
                TextField.text = text;
            }
            catch (Exception ex)
            {

            }
            ShouldUpdate = false;
        }
    }



    void UpdateTextFieldAndPredictionsButtons(string data)
    {
        data = data.Trim('\r', '\n');
        string[] data1 = data.Split('#');

        if (data1.Length > 0)
            switch (data1[0])
            {
                case "restart":
                    Debug.Log("sentence re-entry");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                      entryProcessing.RestartInput());
                    return;
                case "regenerate":
                    Debug.Log("sentence re-generation");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    entryProcessing.RegenerateSentences());
                    return;
                case "switchA":
                    Debug.Log("switch A");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    switchABCD.switchA());
                    return;
                case "switchB":
                    Debug.Log("switch B");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    switchABCD.switchB());
                    return;
                case "switchC":
                    Debug.Log("switch C");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    switchABCD.switchC());
                    return;
                case "switchD":
                    Debug.Log("switch D");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    switchABCD.switchD());
                    return;
                case "switchTrain":
                    Debug.Log("switch Train");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        if (SceneManagment.isMain)
                            switchABCD.switchMain();
                        else
                            switchABCD.switchTrain();
                    });
                    return;
                case "overlay":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        FindObjectOfType<Overlay>().hideOverlayFlag = !FindObjectOfType<Overlay>().hideOverlayFlag
                    );
                    return;
                case "TorsoReferencing":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    FindObjectOfType<TorsoReferencedContent>().SwitchEnabled());
                    return;
                case "RestartEye":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SRanipal_Eye_Framework.Instance.StopFramework();
                        SRanipal_Eye_Framework.Instance.EnableEye = true;
                        SRanipal_Eye_Framework.Instance.StartFramework();
                    }
                    );
                    return;
                case "StopEye":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SRanipal_Eye_Framework.Instance.StopFramework();
                        SRanipal_Eye_Framework.Instance.EnableEye = false;
                    }
                    );
                    return;
                case "sentences_max5":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            EntryProcessing.SENTENCE_COUNT = 5;
                            EntryProcessing.TRAIN_SENTENCE_COUNT = 5;
                            PlayerPrefs.SetInt("Test_Session_count", EntryProcessing.SENTENCE_COUNT); //Номер попыток
                            PlayerPrefs.SetInt("Session_count", EntryProcessing.SENTENCE_COUNT); //Номер попыток
                        }
                    );
                   

                    return;
                case "sentences_max8":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            EntryProcessing.SENTENCE_COUNT = 8;
                            EntryProcessing.TRAIN_SENTENCE_COUNT = 8;
                            PlayerPrefs.SetInt("Test_Session_count", EntryProcessing.SENTENCE_COUNT); //Номер попыток
                            PlayerPrefs.SetInt("Session_count", EntryProcessing.SENTENCE_COUNT); //Номер попыток
                        }
                    );

                    return;

                case "load_menu":
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            FindObjectOfType<SceneManagment>().LoadMenu();
                        }
                    );
                    return;
                default:
                    break;
            }

        string clientMessage = data1.Aggregate("", (max, cur) => max.Length > cur.Length ? max : cur);
        text = clientMessage.Trim('\r', '\n');

#if UNITY_EDITOR
        server.responseDelay.Stop();
        Debug.Log($"RESPONSE DELAY: {server.responseDelay.ElapsedMilliseconds.ToString()}");
#endif

        ShouldUpdate = true;
    }

}