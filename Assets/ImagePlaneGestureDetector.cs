using LeapMotionGesture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePlaneGestureDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    AirStrokeMapper airStrokeMapper;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ImagePlaneThumb"))
        {
            airStrokeMapper.OnPinchBegan();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ImagePlaneThumb"))
        {
            airStrokeMapper.OnPinchEnded();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        airStrokeMapper = FindObjectOfType<AirStrokeMapper>();
    }
}
