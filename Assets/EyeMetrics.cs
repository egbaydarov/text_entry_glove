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
        { //keyboard surface
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.check_time_eye += measuringMetrics.check_time_sw_eye.ElapsedMilliseconds;
                measuringMetrics.check_time_sw_eye.Reset();
                measuringMetrics.entry_time_sw_single.Start();
                //measuringMetrics.entry_time_sw.Start(); upd12 2
                measuringMetrics.search_time_sw_single.Start();
                measuringMetrics.search_time_sw.Start();
            }
        }
        else
        { //prediction surface
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.check_time_sw_eye.Restart();
                measuringMetrics.entry_time_sw_single.Stop();
                //measuringMetrics.entry_time_sw.Stop(); upd12 2
                measuringMetrics.search_time_sw_single.Stop();
                measuringMetrics.search_time_sw.Stop();
                //начало замера времени коррецкции слово может быть ошибочным
                measuringMetrics.remove_time_sw.Restart();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isMainKeys)
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                //начало замера времени коррецкции слово может быть ошибочным
                measuringMetrics.remove_time_sw.Restart();

              
            }
        }
        else
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                //measuringMetrics.check_time_eye += measuringMetrics.check_time_sw_eye.ElapsedMilliseconds;
                //measuringMetrics.check_time_sw_eye.Reset();
                //measuringMetrics.entry_time_sw_single.Start();
                ////measuringMetrics.entry_time_sw.Start(); upd12 2
                //measuringMetrics.search_time_sw_single.Start();
                //measuringMetrics.search_time_sw.Start();
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
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Awake()
    {
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
    }
}
