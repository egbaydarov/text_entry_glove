using LeapMotionGesture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePlaneGestureDetector : MonoBehaviour
{
    public bool IsTriggered { get; set; } = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    AirStrokeMapper airStrokeMapper;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("ImagePlaneThumb"))
        {
            IsTriggered = true;
            airStrokeMapper.OnPinchBegan();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ImagePlaneThumb"))
        {
            IsTriggered = false;
            var AllTrigers = FindObjectsOfType<ImagePlaneGestureDetector>();
            foreach(var trigger in AllTrigers)
            {
                if(trigger.IsTriggered)
                {
                    return;
                }
            }
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
