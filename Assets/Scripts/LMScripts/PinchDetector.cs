using UnityEngine;
using UnityEngine.Events;

namespace LeapMotionGesture
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PinchDetector : MonoBehaviour
    {
        public UnityEvent PinchBegan = new UnityEvent();
        public UnityEvent PinchEnded = new UnityEvent();

        private void Start()
        {
            if (!GetComponent<Collider>().isTrigger)
            {
                Debug.LogError("PinchDetector: A collider of this object should be a trigger. Disabling the script");
                enabled = false;
                return;
            }

            if (!GetComponent<Rigidbody>().isKinematic)
            {
                Debug.LogError("PinchDetector: A rigidbody of this object should be kinematic. Disabling the script");
                enabled = false;
                return;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PinchBegan.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            PinchEnded.Invoke();
        }
    }
}
