using LeapMotionGesture;
using UnityEngine;

public class PinchWithReticle : MonoBehaviour
{
    // Start is called before the first frame update

    Canvas keyCanvas;
    GameObject reticlePointer;
    GameObject LMPointer;

    public bool HideReticleWhileGesturing = true;
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        if (LMPointer != null)
            if (AirStrokeMapper.pinchIsOn)
            {
                keyCanvas.worldCamera = LMPointer.GetComponent<Camera>();
                LMPointer.SetActive(true);
                if (HideReticleWhileGesturing)
                    reticlePointer.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                keyCanvas.worldCamera = reticlePointer.GetComponent<Camera>();
                LMPointer.SetActive(false);
                if (HideReticleWhileGesturing)
                    reticlePointer.GetComponent<MeshRenderer>().enabled = true;
            }

    }

    private void Awake()
    {
        reticlePointer = GameObject.FindGameObjectWithTag("ReticlePointer");
        LMPointer = GameObject.FindGameObjectWithTag("LMPointer");
        keyCanvas = GameObject.FindGameObjectWithTag("KeyboardCanvas").GetComponent<Canvas>();
    }
}
