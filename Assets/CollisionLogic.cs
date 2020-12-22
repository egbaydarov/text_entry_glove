using System;
using System.Collections;
using System.Collections.Generic;
using LeapMotionGesture;
using UnityEngine;

public class CollisionLogic : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        AirStrokeMapper.pinchIsOn = true;
        Debug.Log("COLLISION ENTER");
    }

    private void OnCollisionExit(Collision other)
    {
        AirStrokeMapper.pinchIsOn = false;
        Debug.Log("COLLISION EXIT");
    }
}
