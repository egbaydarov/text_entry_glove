using System.Collections;
using System.Collections.Generic;
using TextEntry;
using UnityEngine;

public class Trigerring : MonoBehaviour
{
    private MeshRenderer mr;    
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (SerialCommunication.buttonState)
            mr.material.SetColor("_Color", Color.magenta);
        else
            mr.material.SetColor("_Color", Color.white);
    }
}
