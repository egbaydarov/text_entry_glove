using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColiderReticle : MonoBehaviour
{

    public bool Triggering { get; set; } = false;

    EntryProcessing entryProcessing;

    public Vector3 startPos = new Vector3();

    int hoverCounter = 0;

    Server server;
    MeasuringMetrics measuringMetrics;
    [SerializeField]
    UnityEvent OnTriggerEnterEvent;
    [SerializeField]
    UnityEvent OnTrigerExit;
    public GameObject Go { get; set; }
    GameObject FakePointer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        server = FindObjectOfType<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
        entryProcessing = FindObjectOfType<EntryProcessing>();
        FakePointer = GameObject.Find("Fake Pointer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Go != null && Triggering)
        {
            float x = transform.InverseTransformPoint(Go.gameObject.transform.position).x;
            float y = transform.InverseTransformPoint(Go.gameObject.transform.position).y;
            startPos = transform.TransformPoint(new Vector3(x, y, 0));

            Vector3 local = FakePointer.transform.parent.InverseTransformPoint
            //(LastPointerHoveredResult.gameObject.GetComponent<Transform>().position); // - centre
            //(GetClosestRaycast(delay).gameObject.GetComponent<Transform>().position);
            (startPos);

            FakePointer.transform.localPosition = new Vector3(local.x, local.y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tapTip"))
        {
            Go = other.gameObject;
            float x = transform.InverseTransformPoint(other.gameObject.transform.position).x;
            float y = transform.InverseTransformPoint(other.gameObject.transform.position).y;
            startPos = transform.TransformPoint(new Vector3(x, y, 0));

            Vector3 local = FakePointer.transform.parent.InverseTransformPoint
            //(LastPointerHoveredResult.gameObject.GetComponent<Transform>().position); // - centre
            //(GetClosestRaycast(delay).gameObject.GetComponent<Transform>().position);
            (startPos);

            FakePointer.transform.localPosition = new Vector3(local.x, local.y, 0);
            Triggering = true;
        }

        //TO DO CHANGE HAND COLOR

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("tapTip"))
        {
            Triggering = false;
            FakePointer.transform.position = new Vector3(-100000, -10000, -100000);
        }
        //TO DO CHANGE HAND COLOR

    }
}
