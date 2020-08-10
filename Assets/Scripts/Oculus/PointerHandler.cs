using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHandler : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    Transform cameraTransform;

    [SerializeField]
    Vector3 shoulderOffset = Vector3.zero;

    [SerializeField]
    float damping = 1.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lookAt();
        transform.position = cameraTransform.position + shoulderOffset;
    }

    void lookAt()
    {
        Vector3 delta = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(delta);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

    }

}
