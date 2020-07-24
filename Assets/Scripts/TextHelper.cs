using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper: MonoBehaviour
{
    [SerializeField]
    private InputField intext;
    // Prediction buttons 

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
            intext.text = Server.mytext;
            Server.isTextUpdated = false;
        }
        
    }
}