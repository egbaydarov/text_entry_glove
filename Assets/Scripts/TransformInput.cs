using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

namespace TextEntry
{
//0.808960 0.310852 0.481384 0.130676 9.925030 -59.325920 -47.714153 1 0 up
//0.732239 -0.221497 0.490479 -0.417358 66.600174 -64.579483 -11.426326 1 0 down
//0.195313 -0.173096 0.942261 -0.209717 -164.208830 -26.146814 -158.595490 1 0 right
//0.973694 0.061035 0.177185 -0.129456 16.853054 -19.222975 -10.048240 1 0 left
    [RequireComponent(typeof(SerialCommunication))]
    public class TransformInput : MonoBehaviour
    {

        // Start is called before the first frame update
        public Camera hmd;
        public GameObject sphere;
        public GameObject reticle;

        public Vector3 camera_offset = new Vector3(0.25f, -0.31f, 0.6f);
        public InputMode mode = InputMode.Rotation;
        public Quaternion local_zero;


        public int Height = 900;
        public int Width = 3000;
        public Vector3 up = new Vector3(30, -59, -47);//110
        public Vector3 down = new Vector3(170, -64, -11);
        public Vector3 right = new Vector3(-164, -40, -158);//-10
        public Vector3 left = new Vector3(16,70,-10);

        void Start()
        {
            //transform.position = hmd.transform.position + camera_offset;
            //transform.rotation = hmd.transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {

            switch (mode)
            {

                case InputMode.Rotation:
                    transform.rotation = SerialCommunication.cRotation;
                    sphere.transform.position = reticle.transform.position;
                    break;
                case InputMode.Position:
                    transform.localPosition = rotation_to_position(SerialCommunication.cRotation);
                    break;
                case InputMode.Both:
                    transform.rotation = SerialCommunication.cRotation;
                    transform.localPosition = SerialCommunication.cPosition;
                    break;
            }
        }

        Vector3 rotation_to_position(Quaternion rot)
        {
            float x = rot.eulerAngles.x - 10;//0 - 140
            float y = rot.eulerAngles.y + 40;//0 - 110
            return new Vector3(x / 140 * Width, y / 110 * Height, -1000);
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

