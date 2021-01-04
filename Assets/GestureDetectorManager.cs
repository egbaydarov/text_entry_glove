using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetectorManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    ImagePlaneGestureDetector gd1;
    [SerializeField]
    ImagePlaneGestureDetector gd2;
    [SerializeField]
    ImagePlaneGestureDetector gd3;
    [SerializeField]
    ImagePlaneGestureDetector gd4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gd1.enabled = enabled;
        gd2.enabled = enabled;
        gd3.enabled = enabled;
        gd4.enabled = enabled;
    }
}
