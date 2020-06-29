using LeapMotionGesture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchWithReticle : MonoBehaviour
{
    // Start is called before the first frame update

    Canvas keyCanvas;
    GameObject reticlePointer;
    GameObject LMPointer;
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if(AirStrokeMapper.pinchIsOn)
        {
            keyCanvas.worldCamera = LMPointer.GetComponent<Camera>();
            LMPointer.SetActive(true);
            reticlePointer.GetComponent<MeshRenderer>().enabled  = false;
        }
        else
        {
            keyCanvas.worldCamera = reticlePointer.GetComponent<Camera>();
            LMPointer.SetActive(false);
            reticlePointer.GetComponent<MeshRenderer>().enabled  = true;
        }

    }

    private void Awake()
    {
        reticlePointer = GameObject.FindGameObjectWithTag("ReticlePointer");
        LMPointer = GameObject.FindGameObjectWithTag("LMPointer");
        keyCanvas = GameObject.FindGameObjectWithTag("KeyboardCanvas").GetComponent<Canvas>();
    }
}
