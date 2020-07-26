using Boo.Lang;
using TextEntry;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(TrailRender))]
public class GlovePointer : GvrBasePointer
{
    /// <summary>Maximum distance from the pointer that raycast hits will be detected.</summary>
    [Tooltip("Distance from the pointer that raycast hits will be detected.")]
    public float maxPointerDistance = 20.0f;

    /// <summary>
    /// Distance from the pointer that the reticle will be drawn at when hitting nothing.
    /// </summary>
    [Tooltip("Distance from the pointer that the reticle will be drawn at when hitting nothing.")]
    public float defaultReticleDistance = 20.0f;

    /// <summary>
    /// By default, the length of the laser is used as the CameraRayIntersectionDistance.
    /// </summary>
    /// <remarks>Set this field to a non-zero value to override it.</remarks>
    [Tooltip("By default, the length of the laser is used as the CameraRayIntersectionDistance. " +
    "Set this field to a non-zero value to override it.")]
    public float overrideCameraRayIntersectionDistance;

    public GameObject pointer_dot;

    TrailRender trRander;
    /// <inheritdoc/>
    public override float MaxPointerDistance
    {
        get
        {
            return maxPointerDistance;
        }
    }

    /// <inheritdoc/>
    public override float CameraRayIntersectionDistance
    {
        get
        {
            if (overrideCameraRayIntersectionDistance != 0.0f)
            {
                return overrideCameraRayIntersectionDistance;
            }

            return  overrideCameraRayIntersectionDistance;
        }
    }

    /// <inheritdoc/>
    public override void OnPointerEnter(RaycastResult raycastResult, bool isInteractive)
    {

    }

    /// <inheritdoc/>
    public override void OnPointerHover(RaycastResult raycastResult, bool isInteractive)
    {
        pointer_dot.transform.position = raycastResult.worldPosition;

        Server.x = raycastResult.worldPosition.x;
        Server.y = raycastResult.worldPosition.y;
        if (SerialCommunication.buttonState)
        {
            GameObject trailPoint = new GameObject();
            trailPoint.transform.position = raycastResult.worldPosition + new Vector3(0, 0, -50);

            trRander.AddPoint(trailPoint);
        }

    }

    /// <inheritdoc/>
    public override void OnPointerExit(GameObject previousObject)
    {

    }

    /// <inheritdoc/>
    public override void OnPointerClickDown()
    {
        
        Server.OnPointerDown();

        if(!(Server.x > -530 && Server.y < -100 && Server.x < -450 && Server.y > -220))
        {
            Shift.SizeReset();
        }
    }

    /// <inheritdoc/>
    public override void OnPointerClickUp()
    {
        Server.OnPointerUp();
        trRander.RemoveTrail();
    }

    /// <inheritdoc/>
    public override void GetPointerRadius(out float enterRadius, out float exitRadius)
    {
        enterRadius = 0.0f;
        exitRadius = 0.0f;
    }

    protected override void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        pointer_dot = GameObject.FindWithTag("KeyboardDot");
        trRander = GetComponent<TrailRender>();
    }

    private void Update()
    {
        
    }

    public override bool TriggerUp => !SerialCommunication.buttonState;
    public override bool Triggering => SerialCommunication.buttonState;
    public override bool TriggerDown => SerialCommunication.buttonState;
}
