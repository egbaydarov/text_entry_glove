using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextHelper: MonoBehaviour
{
    public Text intext;
    public GameObject go;
    private Server server;


    private void Awake()
    {
        server = go.GetComponent<Server>();
    }

    // Start is called before the first frame update
    void Start()
    {
        go = GameObject.Find("keyboard");
    }

    // Update is called once per frame
    void Update()
    {
        intext.text = server.mytext.Split('#')[0];
    }
}