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
        transform.position = Vector3.Lerp(transform.position, (wrist.position + palm.position) / 2, smoothing * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, palm.rotation * Quaternion.Euler(offset), Time.deltaTime * rotationSmoothing);
    }
}
