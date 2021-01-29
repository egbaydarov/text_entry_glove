using Leap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TrailRender : MonoBehaviour
{
    public Queue<GameObject> trailPoints = new Queue<GameObject>();

    private LineRenderer line;

    public float LINE_WIDTH = 0.01f;

    public Vector3 Drawing_Surface = new Vector3(0, 0, -0.01f);

    void Start()
    {
        //float alpha = 0.9f;
        //Gradient gradient = new Gradient();
        //gradient.SetKeys(
        //    new GradientColorKey[] { new GradientColorKey(Color.magenta, 0.0f), new GradientColorKey(Color.cyan, 1.0f) },
        //    new GradientAlphaKey[] { new GradientAlphaKey(0.01f, 0.0f), new GradientAlphaKey(0.1f, 1.0f) }
        //);
        //line.colorGradient = gradient;
    }

    void Update()
    {
        line.positionCount = trailPoints.Count;
        int i = 0;
        foreach (var point in trailPoints)
            line.SetPosition(i++, point.transform.position);

    }

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.endWidth = LINE_WIDTH;
        line.startWidth = LINE_WIDTH;
    }

    public void setLineWidth(float lineWidth)
    {
        line.endWidth = lineWidth;
        line.startWidth = lineWidth;
    }

    public void AddPoint(GameObject trailPoint)
    {
        trailPoints.Enqueue(trailPoint);
        if (trailPoints.Count > 30)
            Destroy(trailPoints.Dequeue());
    }

    public void RemoveTrail()
    {
        foreach (var point in trailPoints)
            Destroy(point);
        trailPoints.Clear();
    }
}
