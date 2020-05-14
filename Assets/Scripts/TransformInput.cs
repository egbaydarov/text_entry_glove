using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

namespace TextEntry
{
    [RequireComponent(typeof(SerialCommunication))]
    public class TransformInput : MonoBehaviour
    {
        // Start is called before the first frame update
        public Camera hmd;
        public Vector3 camera_offset = new Vector3(0.25f, -0.31f, 0.6f);
        public InputMode mode = InputMode.Rotation;
        public Quaternion local_zero;

        void Start()
        {
            transform.position = hmd.transform.position + camera_offset;
            transform.rotation = hmd.transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            switch (mode)
            {

                case InputMode.Rotation:
                    transform.rotation = SerialCommunication.cRotation;
                    break;
                case InputMode.Position:
                    transform.localPosition = SerialCommunication.cPosition;
                    break;
                case InputMode.Both:
                    transform.rotation = SerialCommunication.cRotation;
                    transform.localPosition = SerialCommunication.cPosition;
                    break;
            }
        }

        public void position_zeroing()
        {
            transform.position = hmd.transform.TransformPoint(camera_offset);
            transform.rotation = hmd.transform.rotation;
        }

    }
}

public enum InputMode
{
    Rotation,
    Position,
    Both
}

