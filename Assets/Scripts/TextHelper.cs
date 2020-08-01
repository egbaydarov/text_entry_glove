using System.Collections;
using System.Collections.Generic;
using System.Text;
using Leap.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper: MonoBehaviour
{
    [SerializeField]
    private InputField intext;
    // Prediction buttons 
    [SerializeField] private Button button0;
    [SerializeField] private Button button1;
    [SerializeField] private Button button2;
    [SerializeField] private Text prediction0;
    [SerializeField] private Text prediction1;
    [SerializeField] private Text prediction2;
    
    private void Awake()
    {
        //server = go.GetComponent<Server>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // parse mytext in array, middle word goes to intext 
        if (Server.isTextUpdated)
        {
            if (intext.text == "")
            {
                for (var i = 0; i < Server.predictions.Length; i++)
                {
                    Server.predictions[i] = Server.predictions[i].Capitalize();
                }

                intext.text = Server.predictions[1];
                prediction0.text = Server.predictions[0];
                prediction1.text = Server.predictions[1];
                prediction2.text = Server.predictions[2];
            }
            else
            {

                intext.text += " " + Server.predictions[1];
                prediction0.text = Server.predictions[0];
                prediction1.text = Server.predictions[1];
                prediction2.text = Server.predictions[2];
            }

            Server.isTextUpdated = false;
        }
        
    }

    public void ChangeOnPrediction0()
    {
        intext.text = intext.text.Remove(intext.text.Length - Server.predictions[1].Length) + Server.predictions[0];
    }
    public void ChangeOnPrediction1()
    {
        intext.text = intext.text.Remove(intext.text.Length - Server.predictions[1].Length) + Server.predictions[1];
    }
    public void ChangeOnPrediction2()
    {
        intext.text = intext.text.Remove(intext.text.Length - Server.predictions[1].Length) + Server.predictions[2];
    }
}