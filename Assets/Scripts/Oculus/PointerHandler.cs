using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHandler : MonoBehaviour
{
    [SerializeField]
    Transform targetLeft;

    [SerializeField]
    Transform targetRight;

    Transform target;

    [SerializeField]
    Transform cameraTransform;


    [SerializeField]
    Vector3 shoulderOffset = Vector3.zero;

    [SerializeField]
    float damping = 1.0f;

    [SerializeField]
    float laserWidth = 0.06f;

    [SerializeField]
    LineRenderer laser;

    void Start()
    {
        laser.positionCount = 3;
        laser.endWidth = laserWidth;
        laser.startWidth = laserWidth;
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

        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, target.position);
        laser.SetPosition(2, GetComponent<ReticlePointer>().ReticleWorldPosition);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

    }

    private void Awake()
    {

        if (FindObjectOfType<PinchDetectorDelay>().PinchHand == PinchDetectorDelay.HandMode.right)
        {
            target = targetRight;
            shoulderOffset.x = Mathf.Abs(shoulderOffset.x);
        }
        else
        {
            target = targetLeft;
            shoulderOffset.x = -Mathf.Abs(shoulderOffset.x);
        }
    }



}
