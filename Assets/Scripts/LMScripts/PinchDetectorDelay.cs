using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PinchDetectorDelay : MonoBehaviour
{
    [SerializeField]
    PinchDetector detector;

    [SerializeField]
    int delay = 3;

    private void Start()
    {
        if(detector == null)
        {
            Debug.LogError("Empty instance of pinch detector.");
            enabled = false;
            return;
        }

        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        detector.enabled = true;
    }
}
