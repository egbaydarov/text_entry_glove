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
                measuringMetrics.check_time_eye += measuringMetrics.check_time_sw_eye.ElapsedMilliseconds;
                measuringMetrics.check_time_sw_eye.Reset();

                measuringMetrics.search_time_sw_eye.Start();

                measuringMetrics.remove_time_sw.Restart();
            }
        }
        else
        { //prediction surface enter
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                measuringMetrics.check_time_sw_eye.Restart();

                measuringMetrics.search_time_sw_eye.Stop();
                measuringMetrics.search_time_eye += measuringMetrics.search_time_sw_eye.ElapsedMilliseconds;
                measuringMetrics.search_time_sw_eye.Reset();

                measuringMetrics.remove_time_sw.Restart();
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
                //начало замера времени коррецкции слово может быть ошибочным
                //measuringMetrics.remove_time_sw.Restart(); решил использовать только события входа в плоскость
            }
        }
        else
        { //prediction surface exit
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
