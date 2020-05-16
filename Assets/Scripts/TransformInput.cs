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
        public Camera hmd;
        public GameObject pointer;
        public GameObject shoulder;

        public InputMode mode = InputMode.Rotation;
        public Quaternion local_zero = Quaternion.identity;


        void Update()
        {

            switch (mode)
            {

                case InputMode.Rotation:
                    transform.rotation = Quaternion.Inverse(local_zero) * SerialCommunication.cRotation;
                    pointer.transform.position = shoulder.transform.position;
                    break;
                case InputMode.Position:
                    break;
                case InputMode.Both:
                    break;
            }
        }

        public void position_zeroing()
        {
            local_zero = SerialCommunication.cRotation;
        }

    }
}

public enum InputMode
{
    Rotation,
    Position,
    Both
}

