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

    private Transform transform;
    
    private GameObject canvas;
    
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

        Server.x = transform.InverseTransformPoint(raycastResult.worldPosition).x;
        Server.y = transform.InverseTransformPoint(raycastResult.worldPosition).y;
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

        float x_min = -1080/2 +10;
        float x_max = -1080/2 +10 + (1080-120)/11;
        float y_min = -660/2+(float)(0.835*660-45)/4 + 20;
        float y_max = -660/2+(float)(0.835*660-45)/2 + 20;
        Debug.Log(x_min + " " + x_max + " " + " "+ y_min+ y_max);
        if (!(Server.x > x_min && Server.y < y_max && Server.x < x_max && Server.y > y_min))
        {
            Server.shiftReset();
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
        canvas = GameObject.Find("CanvasKeyboard");
        transform = canvas.transform;
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
