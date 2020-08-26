using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestureDetector : MonoBehaviour
{
    private bool _isPointing;
    public UnityEvent OnActivate;
    public UnityEvent OnDeactivate;


    public bool isPointing
    {
        get => _isPointing;
        set
        {
            if (value == false)
            {
                isGestureActivated = false;
                OnDeactivate.Invoke();
            }
            _isPointing = value;
        }
    }

    public bool isGestureActivated { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (isPointing && collision.gameObject.tag.Equals("ThumbFinger") && !isGestureActivated)
        {
            OnActivate.Invoke();
            isGestureActivated = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    public void Log(string mess)
    {
        Debug.Log(mess);
    }

}
