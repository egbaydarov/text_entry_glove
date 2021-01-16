using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading;
using UnityEngine;

public class PinchDetectorDelay : MonoBehaviour
{
    [SerializeField]
    MonoBehaviour leftPinchDetector;

    [SerializeField]
    MonoBehaviour rightPinchDetector;

    [SerializeField]
    GestureDetectorManager leftGDM;
    [SerializeField]
    GestureDetectorManager rightGDM;

    [SerializeField]
    GameObject pointerRig;

    [SerializeField]
    public HandMode PinchHand = HandMode.right;


    [SerializeField]
    int delay = 3;

    bool InWaiting;

    private void Start()
    {
        if (rightPinchDetector == null || leftPinchDetector == null)
        {
            Debug.LogError("Empty instance of Hand Models GO.");
            enabled = false;
            return;
        }

        StartCoroutine(ExecuteAfterTime(delay));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        InWaiting = true;
        yield return new WaitForSeconds(time);

        InWaiting = false;
        if (PinchHand == HandMode.right)
        {
            rightPinchDetector.enabled = true;
            if(rightGDM != null)
            {
                rightGDM.isEnabled = true;
            }
            if (pointerRig != null && rightPinchDetector.gameObject.activeInHierarchy)
                pointerRig.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            leftPinchDetector.enabled = true;
            if (leftGDM != null)
            {
                leftGDM.isEnabled = true;
            }
            if (pointerRig != null && leftPinchDetector.gameObject.activeInHierarchy)
                pointerRig.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void DisablePinchForSeconds(float seconds)
    {
        if (PinchHand == HandMode.right)
        {
            rightPinchDetector.enabled = false;
            if (rightGDM != null)
            {
                rightGDM.isEnabled = false;
            }
            if (pointerRig != null && rightPinchDetector.gameObject.activeInHierarchy)
                pointerRig.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            leftPinchDetector.enabled = false;
            if (leftGDM != null)
            {
                leftGDM.isEnabled = false;
            }
            if (pointerRig != null && leftPinchDetector.gameObject.activeInHierarchy)
                pointerRig.GetComponent<MeshRenderer>().enabled = false;
        }
        StartCoroutine(ExecuteAfterTime(seconds));
    }

    //используется в скрипте HandEnableDisable
    public void SetReticleMeshEnabled(bool value)
    {
        //В случае ожтдания трех екунд при запуске сцены и после нажатия на клавишу ввод завершен
        if (value && InWaiting)
            return;

        if (pointerRig != null)
            pointerRig.GetComponent<MeshRenderer>().enabled = value;
    }

    public enum HandMode
    {
        left,
        right
    }
}
