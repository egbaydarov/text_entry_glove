using System.Collections;
using System.Collections.Generic;
using TextEntry;
using UnityEngine;

public class Trigerring : MonoBehaviour
{
    public MeshRenderer mr;    
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (SerialCommunication.buttonState)
            mr.material.SetColor("_Color", Color.blue);
        else
            mr.material.SetColor("_Color", Color.white);
    }
}
