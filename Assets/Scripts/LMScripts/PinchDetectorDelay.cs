using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PinchDetectorDelay : MonoBehaviour
{
    [SerializeField]
    PinchDetector leftPinchDetector;

    [SerializeField]
    PinchDetector rightPinchDetector;

    [SerializeField]
    GameObject pointerRig;

    [SerializeField]
    public HandMode PinchHand = HandMode.right;


    [SerializeField]
    int delay = 3;

    private void Start()
    {
        if(rightPinchDetector == null || leftPinchDetector == null)
        {
            Debug.LogError("Empty instance of Hand Models GO.");
            enabled = false;
            return;
        }

        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);


        if (PinchHand == HandMode.right)
        {
            rightPinchDetector.enabled = true;
        }
        else
        {
            leftPinchDetector.enabled = true;
        }

        if (pointerRig != null)
            pointerRig.SetActive(true);
    }

    public void DisablePinchForSeconds(float seconds)
    {
        if (PinchHand == HandMode.right)
        {
            rightPinchDetector.enabled = false;
        }
        else
        {
            leftPinchDetector.enabled = false;
        }

        if (pointerRig != null)
            pointerRig.SetActive(false);

        StartCoroutine(ExecuteAfterTime(seconds));
    }

    public enum HandMode
    {
        left,
        right
    }
}
