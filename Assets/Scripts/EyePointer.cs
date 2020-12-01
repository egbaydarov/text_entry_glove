
using Leap;
using LeapMotionGesture;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EyePointer : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    float RaycastDistance = 1000;

    [SerializeField]
    Camera EventCamera;

    [SerializeField]
    GraphicRaycaster Raycaster;
    

    private void Start()
    {
        if (EventCamera == null)
        {
            Debug.LogError("Event Camera not set in inspector.");
            enabled = false;
        }
    }

    private void Update()
    {
        //Physics.Raycast(EventCamera.transform.position, EventCamera.transform.forward, out hit, RaycastDistance);

        //PointerEventData ed = new PointerEventData(null);
        //ed.position = Input.mousePosition;
        //List<RaycastResult> raycastResults = new List<RaycastResult>();
        
        //Raycaster.Raycast(ed, raycastResults);
  }
    public void message()
    {
        Debug.Log("HI");
    }
}
