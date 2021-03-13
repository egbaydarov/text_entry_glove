using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

public class PointerDirection : MonoBehaviour
{
    
private readonly GazeIndex[] GazePriority = new GazeIndex[] { GazeIndex.COMBINE, GazeIndex.LEFT, GazeIndex.RIGHT };
private static EyeData eyeData = new EyeData();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GazeIndex index in GazePriority)
        {
            Ray GazeRay;
            if (SRanipal_Eye.GetGazeRay(index, out GazeRay, eyeData))
            {
                Debug.Log("true");
            }
            transform.rotation = Quaternion.LookRotation(Vector3.forward, GazeRay.direction);
        }
        
    }
}
