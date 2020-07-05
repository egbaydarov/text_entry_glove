using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TrailRender : MonoBehaviour
{
    List<GameObject> trailPoints = new List<GameObject>();

    private LineRenderer line;

    public float LINE_WIDTH;

    void Start()
    {
        
    }

    void Update()
    {
        line.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; ++i)
            line.SetPosition(i, trailPoints[i].transform.position);
    }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.endWidth = LINE_WIDTH;
        line.startWidth = LINE_WIDTH;
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
