using LeapMotionGesture;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>Implementation of GvrBasePointer for a laser pointer visual.</summary>
/// <remarks>
/// This script should be attached to the controller object. The laser visual is important to help
/// users locate their cursor when its not directly in their field of view.
/// </remarks>
[RequireComponent(typeof(GvrLaserVisual))]
public class DirectLazerPointer : GvrBasePointer
{
    public TrailRender trRander;
    private Transform trLocal;
    private GameObject canvas;



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

    /// <summary>The percentage of the reticle mesh that shows the reticle.</summary>
    /// <remarks>The rest of the reticle mesh is transparent.</remarks>
    private const float RETICLE_VISUAL_RATIO = 0.1f;

    private bool isHittingTarget;

    /// <summary>Gets the visual object for the laser beam.</summary>
    /// <value>The visual object for the laser beam.</value>
    public GvrLaserVisual LaserVisual { get; private set; }

    public GameObject cursor;

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

            return LaserVisual != null ?
                                  LaserVisual.maxLaserDistance :
                                  overrideCameraRayIntersectionDistance;
        }
    }

    /// <inheritdoc/>
    public override void OnPointerEnter(RaycastResult raycastResult, bool isInteractive)
    {
        LaserVisual.SetDistance(raycastResult.distance);
        isHittingTarget = true;
    }

    /// <inheritdoc/>
    public override void OnPointerHover(RaycastResult raycastResult, bool isInteractive)
    {
        LaserVisual.SetDistance(raycastResult.distance);
        isHittingTarget = true;


        Server.x = trLocal.InverseTransformPoint(raycastResult.worldPosition).x;
        Server.y = trLocal.InverseTransformPoint(raycastResult.worldPosition).y;
        if (Triggering)
        {
            GameObject trailPoint = new GameObject();
            trailPoint.transform.position = raycastResult.worldPosition + trRander.Drawing_Surface;
            trailPoint.transform.SetParent(canvas.transform);

            trRander.AddPoint(trailPoint);
        }
    }

    /// <inheritdoc/>
    public override void OnPointerExit(GameObject previousObject)
    {
        // Don't set the distance immediately.
        // If we exit/enter an object on the same frame, then SetDistance
        // will be called twice which could cause an issue with lerping the reticle.
        // If we don't re-enter a new object, the distance will be set in Update.
        isHittingTarget = false;
    }

    /// <inheritdoc/>
    public override void OnPointerClickDown()
    {
        Server.OnPointerDown();


        float x_min = -1080 / 2 + 10;
        float x_max = -1080 / 2 + 10 + (1080 - 120) / 11;
        float y_min = -660 / 2 + (float)(0.835 * 660 - 45) / 4 + 20;
        float y_max = -660 / 2 + (float)(0.835 * 660 - 45) / 2 + 20;
        Debug.Log(x_min + " " + x_max + " " + " " + y_min + y_max);
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
        if (LaserVisual.reticle != null)
        {
            float reticleScale = LaserVisual.reticle.transform.localScale.x;

            // Fixed size for enter radius to avoid flickering.
            // This will cause some slight variability based on the distance of the object
            // from the camera, and is optimized for the average case.
            enterRadius = LaserVisual.reticle.sizeMeters * 0.5f * RETICLE_VISUAL_RATIO;

            // Dynamic size for exit radius.
            // Always correct because we know the intersection point of the object and can
            // therefore use the correct radius based on the object's distance from the camera.
            exitRadius =
                reticleScale * LaserVisual.reticle.ReticleMeshSizeMeters * RETICLE_VISUAL_RATIO;
        }
        else
        {
            enterRadius = 0.0f;
            exitRadius = 0.0f;
        }
    }

    /// @endcond
    /// @cond
    /// <inheritdoc/>
    protected override void Start()
    {
        base.Start();
        LaserVisual.GetPointForDistanceFunction = GetPointAlongPointer;
        LaserVisual.SetDistance(defaultReticleDistance, true);

        canvas = GameObject.Find("CanvasKeyboard");
        trLocal = canvas.transform;
    }

    /// @cond
    /// <summary>This MonoBehavior's Awake method.</summary>
    private void Awake()
    {
        LaserVisual = GetComponent<GvrLaserVisual>();

    }

    /// @endcond
    /// <summary>This MonoBehavior's Update method.</summary>
    private void Update()
    {
        if (isHittingTarget)
        {
            return;
        }

        LaserVisual.SetDistance(defaultReticleDistance);
    }
    public override bool TriggerUp => !AirStrokeMapper.pinchIsOn;
    public override bool Triggering => AirStrokeMapper.pinchIsOn;
    public override bool TriggerDown => AirStrokeMapper.pinchIsOn;
}
