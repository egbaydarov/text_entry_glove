using UnityEngine;

namespace LeapMotionGesture
{
    public class AirStrokeMapper : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField]
        private Transform followee;
        [SerializeField]
        private Transform follower;
#pragma warning restore 649

        [Min(1f)]
        [SerializeField]
        private float multiplicationFactor = 1f;

#pragma warning disable 414
        [Space]
        [TextArea]
        [SerializeField]
        private string reminder = "The Z axis of the GameObject, which holds this component, serves as a normal of a plane to which Followee's movements are projected";
#pragma warning restore 414

        private bool pinchIsOn;
        private Vector2 prevProjectedPoint;


        private void Start()
        {
            if (followee == null)
            {
                Debug.LogError("AirStrokeMapper: The 'Followee' field cannot be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            if (follower == null)
            {
                Debug.LogError("AirStrokeMapper: The 'Follower' field cannot be left unassigned. Disabling the script");
                enabled = false;
                return;
            }

            // Hiding all child GOs
            int i = transform.childCount;
            while (--i >= 0)
                transform.GetChild(i).gameObject.SetActive(false);
        }


        private void Update()
        {
            if (pinchIsOn)
            {
                Vector2 projectedPoint = GetProjectionOnPlane();

                Vector2 delta = projectedPoint - prevProjectedPoint;
                follower.Translate(delta.x * multiplicationFactor, delta.y * multiplicationFactor, 0, follower.parent);

                prevProjectedPoint = projectedPoint;
            }
        }

        public void OnPinchBegan()
        {
            prevProjectedPoint = GetProjectionOnPlane();
            pinchIsOn = true;
        }

        public void OnPinchEnded()
        {
            pinchIsOn = false;
        }

        private Vector2 GetProjectionOnPlane()
        {
            // Projecting a position of our followee to a plane which is at the origin. The plane is rotated as our GO
            Vector3 p = Vector3.ProjectOnPlane(followee.position, transform.forward);

            // Transforming a position of the projected point to local space of our GO, which makes its Z component equal to 0
            p += transform.position;
            return transform.InverseTransformPoint(p);
        }
    }
}
