using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColiderPointer : MonoBehaviour
{
    public bool Triggering { get; set; } = false;

    EntryProcessing entryProcessing;

    public Vector3 startPos = new Vector3();

    int hoverCounter = 0;

    Server server;
    TrailRender trRander;
    MeasuringMetrics measuringMetrics;
    [SerializeField]
    UnityEvent OnTrigerEnter;
    [SerializeField]
    UnityEvent OnTrigerExit;
    public GameObject Go { get; set; }
    GameObject FakePointer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("tapTip"))
        {
            Go = other.gameObject;
            float x = transform.InverseTransformPoint(other.gameObject.transform.position).x;
            float y = transform.InverseTransformPoint(other.gameObject.transform.position).y;
            startPos = transform.TransformPoint(new Vector3(x, y, trRander.Drawing_Surface.z));

            Vector3 local = FakePointer.transform.parent.InverseTransformPoint
            //(LastPointerHoveredResult.gameObject.GetComponent<Transform>().position); // - centre
            //(GetClosestRaycast(delay).gameObject.GetComponent<Transform>().position);
            (startPos);

            FakePointer.transform.localPosition = new Vector3(local.x, local.y, 0);


            OnTrigerEnter.Invoke();
        }

        //TO DO CHANGE HAND COLOR

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("tapTip"))
        {
            OnTrigerExit.Invoke();
        }
        //TO DO CHANGE HAND COLOR

    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Go != null)
        {
            float x = transform.InverseTransformPoint(Go.gameObject.transform.position).x;
            float y = transform.InverseTransformPoint(Go.gameObject.transform.position).y;
            startPos = transform.TransformPoint(new Vector3(x, y, trRander.Drawing_Surface.z));
        }
    }

    private void Awake()
    {
        trRander = GetComponent<TrailRender>();
        server = FindObjectOfType<Server>();
        measuringMetrics = FindObjectOfType<MeasuringMetrics>();
        entryProcessing = FindObjectOfType<EntryProcessing>();
        FakePointer = GameObject.Find("Fake Pointer");
    }
}
