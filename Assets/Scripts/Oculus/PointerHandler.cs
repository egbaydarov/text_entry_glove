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
    public Vector3 shoulderOffset = Vector3.zero;

    [SerializeField]
    float damping = 1.0f;

    [SerializeField]
    float laserWidth = 0.06f;

    [SerializeField]
    float fingerYOffset = 0.06f;

    [SerializeField]
    bool MRTKMode = false;

    void Start()
    {
        if (MRTKMode && PlayerPrefs.GetString("PointEye") != null && PlayerPrefs.GetString("PointEye").Equals("Right"))
        {
            Settings.isRightEye = true;
            shoulderOffset.x = -Mathf.Abs(shoulderOffset.x);


        }
        else
            shoulderOffset.x = Mathf.Abs(shoulderOffset.x);

        if (FindObjectOfType<PinchDetectorDelay>().PinchHand == PinchDetectorDelay.HandMode.right)
        {
            target = targetRight;
        }
        else
        {
            target = targetLeft;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lookAt();
        transform.position = cameraTransform.position + shoulderOffset;
    }

    void lookAt()
    {
        var targetPos = target.position;
        targetPos.y += fingerYOffset;

        Vector3 delta = target.position - transform.position;
        if(MRTKMode)
            delta = targetPos - transform.position;

        Quaternion rotation = Quaternion.LookRotation(delta);

        //if (MRTKMode)
        //    rotation = Quaternion.RotateTowards(cameraTransform.rotation, Quaternion.LookRotation(delta), Quaternion.Angle(cameraTransform.rotation, Quaternion.LookRotation(delta)) * cameraCoeff);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

    }

    private void Awake()
    {
        if (!MRTKMode && FindObjectOfType<PinchDetectorDelay>().PinchHand == PinchDetectorDelay.HandMode.right)
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
