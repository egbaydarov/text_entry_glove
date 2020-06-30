using LeapMotionGesture;
using System.Collections;
using System.Collections.Generic;
using TextEntry;
using UnityEngine;

public class Pinching : MonoBehaviour
{
    private MeshRenderer mr;
    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (AirStrokeMapper.pinchIsOn)
            mr.material.SetColor("_Color", Color.magenta);
        else
            mr.material.SetColor("_Color", Color.white);
    }
}
