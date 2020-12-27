using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyeMetrics : MonoBehaviour
{
    [SerializeField]
    bool isMainKeys = true;

    MeasuringMetrics measuringMetrics;


    private void OnCollisionEnter(Collision collision)
    {
        if (isMainKeys)
        { //keyboard surface enter
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.OnInputEnter();
            }
        }
        else
        { //prediction surface enter
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.OnControlEnter();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isMainKeys)
        {
            //keyboard surface exit
            if (collision.gameObject.name.Equals("EyePointer"))
            {
               
            }
        }
        else
        { //prediction surface exit
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                
            }
        }
    }




    Rigidbody rb = null;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.localPosition;
        rb.detectCollisions = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity != Vector3.zero || rb.angularVelocity != Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.localPosition = pos;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    private void Awake()
    {
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
    }
}
