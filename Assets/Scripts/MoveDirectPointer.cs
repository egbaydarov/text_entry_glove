using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirectPointer : MonoBehaviour
{
    public Transform wrist;
    public Transform palm;
    public float smoothing;
    public float rotationSmoothing;

    public Vector3 offset;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = wrist.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, palm.rotation, Time.deltaTime * rotationSmoothing);
    }
}
