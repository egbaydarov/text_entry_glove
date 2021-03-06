﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LeapMotionGesture
{
    public class AirStrokeMapper : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Transform followeeLeft;
        [SerializeField]
        private Transform followeeRight;

        private Transform followee;

        [SerializeField]
        private Transform follower;
#pragma warning restore 649

        [Header("Multiplication Factor")]
        [SerializeField]
        private bool calculateDynamically = false;

        [Min(1f)]
        [Tooltip("Used when the 'Calculate Dynamically' checkbox is not set")]
        [SerializeField]
        public float fixedValue = 1f;

        [SerializeField]
        private float originPointOffset = 0f;

#pragma warning disable 414
        [Space]
        [TextArea]
        [SerializeField]
        private string reminder = "The Z axis of the GameObject, which holds this component, serves as a normal of a plane to which Followee's movements are projected";
#pragma warning restore 414

        public static bool pinchIsOn;
        private Vector2 prevProjectedPoint;
        private float distanceToObj;
        private Transform mainCamera;
        private Transform keyboardHolder;

        public UnityEvent onPinchOn;
        public UnityEvent onPinchOff;


        private void Start()
        {
            if (followeeLeft == null)
            {
                Debug.LogError("AirStrokeMapper: The 'FolloweeLeft' field cannot be left unassigned. Disabling the script");
                enabled = false;
                return;
            }
            if (followeeRight == null)
            {
                Debug.LogError("AirStrokeMapper: The 'FolloweeRight' field cannot be left unassigned. Disabling the script");
                enabled = false;
                return;
            }
            var pdd = FindObjectOfType<PinchDetectorDelay>();
            if (pdd != null && pdd.PinchHand == PinchDetectorDelay.HandMode.right)
                followee = followeeRight;
            else
                followee = followeeLeft;

            if (follower == null)
            {
                Debug.LogError("AirStrokeMapper: The 'Follower' field cannot be left unassigned. Disabling the script");
                enabled = false;
                return;
            }
            keyboardHolder = follower.root;


            if (calculateDynamically)
            {
                if (Camera.main == null)
                {
                    Debug.LogError("AirStrokeMapper: Couldn't find the main camera in this scence. Disabling the script");
                    enabled = false;
                    return;
                }
                mainCamera = Camera.main.transform;

                Vector3 originPos = mainCamera.position;
                originPos.x += originPointOffset;
                distanceToObj = Vector3.Distance(follower.root.position, originPos);
            }

            // Hiding all child GOs
            int i = transform.childCount;
            while (--i >= 0)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        ColiderPointer cp;

        private void Awake()
        {
            cp = FindObjectOfType<ColiderPointer>();
        }

        private void Update()
        {
            if (pinchIsOn)
            {
                if (SceneManager.GetActiveScene().name.Equals("Articulatedhands_v2"))
                {
                    Vector2 projectedPoint1 = GetProjectionOnPlane();

                    Vector2 delta1 = projectedPoint1 - prevProjectedPoint;
                    follower.Translate(delta1.x, delta1.y, 0, follower.parent);

                    prevProjectedPoint = projectedPoint1;

                    //follower.localPosition = follower.transform.InverseTransformPoint(cp.startPos);
                    return;
                }

                Vector2 projectedPoint = GetProjectionOnPlane();

                Vector2 delta = projectedPoint - prevProjectedPoint;
                follower.Translate(
                    delta.x * GetMultiplicationFactor() * keyboardHolder.localScale.x,
                    delta.y * GetMultiplicationFactor() * keyboardHolder.localScale.y,
                    0,
                    follower.parent);

                prevProjectedPoint = projectedPoint;
            }
            
        }

        public void OnPinchBegan()
        {
            if (!pinchIsOn)
            {
                prevProjectedPoint = GetProjectionOnPlane();
                pinchIsOn = true;
                onPinchOn.Invoke();
            }
        }

        public void OnPinchEnded()
        {
            if (pinchIsOn)
            {
                pinchIsOn = false;
                onPinchOff.Invoke();
            }
        }

        private Vector2 GetProjectionOnPlane()
        {
            // Projecting a position of our followee to a plane which is at the origin. The plane is rotated as our GO
            Vector3 p = Vector3.ProjectOnPlane(followee.position, transform.forward);

            // Transforming a position of the projected point to local space of our GO, which makes its Z component equal to 0
            p += transform.position;
            return transform.InverseTransformPoint(p);
        }

        private float GetMultiplicationFactor()
        {
            if (calculateDynamically)
            {
                Vector3 originPos = mainCamera.position;
                originPos.z += originPointOffset;

                return distanceToObj / Vector3.Distance(followee.position, originPos);
            }
            else
                return fixedValue;
        }
    }
}
