using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider))]
public class Keyboard : MonoBehaviour
{
    private Renderer Renderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Awake()
    {
        Renderer = GetComponent<Renderer>();
        Focus(Vector3.zero);
    }

    public void Focus(Vector3 focusPoint)
    {
        
    }
}
