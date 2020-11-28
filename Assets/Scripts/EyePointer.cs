
using Leap;
using LeapMotionGesture;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class EyePointer : MonoBehaviour
{
    RaycastHit hit;

    [SerializeField]
    float RaycastDistance = 1000;

    [SerializeField]
    Camera EventCamera;
    

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
        Physics.Raycast(EventCamera.transform.position, EventCamera.transform.forward, out hit, RaycastDistance);
  
  }
    public void message()
    {
        Debug.Log("HI");
    }
}
