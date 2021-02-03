using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderPointer : MonoBehaviour
{
    public bool Triggering { get; set; } = false;

    EntryProcessing entryProcessing;
    public bool isGestureValid { get; set; }
    public bool isInputEnd { get; set; } = false;

    int hoverCounter = 0;

    Server server;
    TrailRender trRander;
    MeasuringMetrics measuringMetrics;
    GameObject keyboardColliderGO;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("GestureZone"))
        {
            Triggering = true;
            keyboardColliderGO = other.gameObject;
            Debug.Log("POINTERDOwN COLLIDERKEY");
        }

        //TO DO CHANGE HAND COLOR

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("GestureZone"))
        {
            Triggering = false;
            Debug.Log("POINTERUP COLLIDERKEY");


            if (server.IsConnected && isGestureValid && !isInputEnd)
            {
                server.SendToClient($"u;\r\n");
#if UNITY_EDITOR
                server.responseDelay.Restart();
#endif
                if (entryProcessing.LastTagDown.Equals("Key"))
                {
                    measuringMetrics.EndGesture();
                }

            }
            //server.SendToClient(data + "\r\n");
            hoverCounter = 0;

            isGestureValid = false;

            trRander.RemoveTrail();
        }
        //TO DO CHANGE HAND COLOR

    }

    public void OnPointerHover()
    {
        if (Triggering)
        {
            GameObject trailPoint = new GameObject();


            float x = keyboardColliderGO.transform.InverseTransformPoint(trailPoint.transform.position).x;
            float y = keyboardColliderGO.transform.InverseTransformPoint(trailPoint.transform.position).y;

            trailPoint.transform.position = new Vector3(x, y, trRander.Drawing_Surface.z);
            trRander.AddPoint(trailPoint);


            x = (float)(x * server.coef_x + server.keyboard_x / 2.0);
            y = (float)(-y * server.coef_y + server.screen_y - (server.keyboard_y / 2.0));
            //UnityEngine.Debug.Log("SEND X:" + x + " Y:" + y);

            if (trRander.trailPoints.Count == 1 && server.IsConnected && isGestureValid && !isInputEnd)
            {
                server.SendToClient($"d;{(int)(x)};{(int)(y)};\r\n");


                if (entryProcessing.LastTagDown.Equals("Key"))
                {
                    measuringMetrics.StartGesture();
                }
            }
            else if (++hoverCounter % 1 == 0 && server.IsConnected && isGestureValid && !isInputEnd)
                server.SendToClient($"{(int)(x)};{(int)(y)};\r\n");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        OnPointerHover();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        trRander = GetComponent<TrailRender>();
        server = FindObjectOfType<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
        entryProcessing = FindObjectOfType<EntryProcessing>();
    }
}
