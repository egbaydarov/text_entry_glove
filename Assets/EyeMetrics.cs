using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyeMetrics : MonoBehaviour
{
    [SerializeField]
    bool isMainKeys = true;

    MeasuringMetrics measuringMetrics;


    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (isMainKeys)
        { //keyboard surface enter
            if (other.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.OnInputExit();
            }
        }
        else
        { //prediction surface enter
            if (other.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.OnControlExit();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
    }
}
