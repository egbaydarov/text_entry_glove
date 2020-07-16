using LeapInternal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PointGesture : MonoBehaviour
{
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;

    void Start()
    {
        
    }

    bool leftIndex = false;
    bool rightIndex = false;

    bool isActive = false;


    public void lActivate()
    {
        leftIndex = true;
        if (rightIndex && !isActive)
        {
            isActive = true;
            OnActivate.Invoke();
        }
    }

    public void lDeactivate()
    {
        leftIndex = false;
        if (!rightIndex && isActive)
        {
            isActive = false;
            OnDeactivate.Invoke();
        }
    }

    public void rActivate()
    {
        rightIndex = true;
        if (leftIndex && !isActive)
        {
            isActive = true;
            OnActivate.Invoke();
        }
    }

    public void rDeactivate()
    {
        rightIndex = false;
        if (!leftIndex && isActive)
        {
            isActive = false;
            OnDeactivate.Invoke();
        }
    }
}
