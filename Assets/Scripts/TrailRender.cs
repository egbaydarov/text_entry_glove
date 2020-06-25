using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TrailRender : MonoBehaviour
{
    List<GameObject> trailPoints = new List<GameObject>();

    private LineRenderer line;

    // Start is called before the first frame update
    void Start()
    {
        line.startWidth = 10f;
        line.endWidth = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        line.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; ++i)
            line.SetPosition(i, trailPoints[i].transform.position);
    }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }


    public void AddPoint(GameObject trailPoint)
    {
        trailPoints.Add(trailPoint);    
    }

    public void RemoveTrail()
    {
        trailPoints.ForEach((x) => Destroy(x));
        trailPoints.Clear();
    }
}
