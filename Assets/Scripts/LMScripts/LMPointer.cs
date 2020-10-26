using Boo.Lang;
using Leap.Unity;
using LeapMotionGesture;
using TextEntry;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(TrailRender))]
[HelpURL("https://developers.google.com/vr/unity/reference/class/GvrReticlePointer")]
public class LMPointer : GvrBasePointer
{
    public bool isGestureValid { get; set; }
    public bool isInputEnd { get; set; } = false;

    int hoverCounter = 0;


    Server server;
    GameObject enterRaycastObj;
    private Transform trLocal;
    private GameObject canvas;
    TrailRender trRander;

    /// <summary>
    /// The constants below are expsed for testing. Minimum inner angle of the reticle (in degrees).
    /// </summary>
    public const float RETICLE_MIN_INNER_ANGLE = 0.0f;

    /// <summary>Minimum outer angle of the reticle (in degrees).</summary>
    public const float RETICLE_MIN_OUTER_ANGLE = 0.5f;

    /// <summary>Minimum outer diameter of reticle when Dynamic Reticle Circle is false the reticle.</summary>
    public float StaticReticleOuterDiameter = 0.015f;

    /// <summary>
    /// Angle at which to expand the reticle when intersecting with an object (in degrees).
    /// </summary>
    public const float RETICLE_GROWTH_ANGLE = 1.5f;

    /// <summary>Minimum distance of the reticle (in meters).</summary>
    public const float RETICLE_DISTANCE_MIN = 0.45f;

    /// <summary>Maximum distance of the reticle (in meters).</summary>
    public float maxReticleDistance = 20.0f;

    /// <summary>Number of segments making the reticle circle.</summary>
    public int reticleSegments = 20;

    /// <summary>Growth speed multiplier for the reticle.</summary>
    public float reticleGrowthSpeed = 8.0f;

    /// <summary>Sorting order to use for the reticle's renderer.</summary>
    /// <remarks><para>
    /// Range values come from https://docs.unity3d.com/ScriptReference/Renderer-sortingOrder.html.
    /// </para><para>
    /// Default value 32767 ensures gaze reticle is always rendered on top.
    /// </para></remarks>
    [Range(-32767, 32767)]
    public int reticleSortingOrder = 32767;

    /// <summary>Gets or sets the material used to render the reticle.</summary>
    /// <value>The material used to render the reticle.</value>
    public Material MaterialComp { private get; set; }

    /// <summary>Gets the current inner angle of the reticle (in degrees).</summary>
    /// <remarks>Exposed for testing.</remarks>
    /// <value>The current inner angle of the reticle (in degrees).</value>
    public float ReticleInnerAngle { get; private set; }

    /// <summary>Gets the current outer angle of the reticle (in degrees).</summary>
    /// <remarks>Exposed for testing.</remarks>
    /// <value>The current outer angle of the reticle (in degrees).</value>
    public float ReticleOuterAngle { get; private set; }

    /// <summary>Gets the current distance of the reticle (in meters).</summary>
    /// <remarks>Getter exposed for testing.</remarks>
    /// <value>The current distance of the reticle (in meters).</value>
    public float ReticleDistanceInMeters { get; private set; }

    /// <summary>
    /// Gets the current inner and outer diameters of the reticle, before distance multiplication.
    /// </summary>
    /// <remarks>Getters exposed for testing.</remarks>
    /// <value>
    /// The current inner and outer diameters of the reticle, before distance multiplication.
    /// </value>
    public float ReticleInnerDiameter { get; private set; }

    /// <summary>Gets the current outer diameter of the reticle (in meters).</summary>
    /// <value>The current outer diameter of the reticle (in meters).</value>
    public float ReticleOuterDiameter { get; private set; }

    public GameObject FakePointer;
    RaycastResult LastPointerHoveredResult;
    public ReticleMode mReticleMode = ReticleMode.StaticDot;


    /// <inheritdoc/>
    public override float MaxPointerDistance
    {
        get { return maxReticleDistance; }
    }

    /// <inheritdoc/>
    public override void OnPointerEnter(RaycastResult raycastResultResult, bool isInteractive)
    {
        enterRaycastObj = raycastResultResult.gameObject;
        SetPointerTarget(raycastResultResult.worldPosition, isInteractive);

        //if(raycastResultResult.gameObject != null && raycastResultResult.gameObject.CompareTag(""))
    }

    /// <inheritdoc/>
    public override void OnPointerHover(RaycastResult raycastResult, bool isInteractive)
    {

        SetPointerTarget(raycastResult.worldPosition, isInteractive);
        LastPointerHoveredResult = raycastResult;

        if ((!raycastResult.gameObject.tag.Equals("Key") && !raycastResult.gameObject.tag.Equals("Prediction"))
            || !isGestureValid)
            return;


        if (Triggering)
        {
            GameObject trailPoint = new GameObject();
            trailPoint.transform.position = raycastResult.worldPosition + trRander.Drawing_Surface;
            trailPoint.transform.SetParent(canvas.transform);

            trRander.AddPoint(trailPoint);

            float x = trLocal.InverseTransformPoint(trailPoint.transform.position).x;
            float y = trLocal.InverseTransformPoint(trailPoint.transform.position).y;
            x = (float)(x * server.coef_x + server.keyboard_x / 2.0);
            y = (float)(-y * server.coef_y + server.screen_y - (server.keyboard_y / 2.0));

            if (trRander.trailPoints.Count == 1 && server.IsConnected && isGestureValid && !isInputEnd)
                server.SendToClient($"d;{(int)(x)};{(int)(y)};\r\n");
            else if (++hoverCounter % 1 == 0 && server.IsConnected && isGestureValid && !isInputEnd)
                server.SendToClient($"{(int)(x)};{(int)(y)};\r\n");
        }

    }

    /// <inheritdoc/>
    public override void OnPointerExit(GameObject previousObject)
    {
        ReticleDistanceInMeters = maxReticleDistance;
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
    }

    /// <inheritdoc/>
    public override void OnPointerClickDown()
    {
        
        isGestureValid = enterRaycastObj.tag.Equals("Key") || enterRaycastObj.tag.Equals("Prediction");
        
        Vector3 local = FakePointer.transform.parent.InverseTransformPoint
            (LastPointerHoveredResult.gameObject.GetComponent<Transform>().position);
        FakePointer.transform.localPosition = new Vector3(local.x, local.y, 0);
    }

    /// <inheritdoc/>
    public override void OnPointerClickUp()
    {
        string data = "";

        foreach (var tp in trRander.trailPoints)
        {
            float x = trLocal.InverseTransformPoint(tp.transform.position).x;
            float y = trLocal.InverseTransformPoint(tp.transform.position).y;
            x = (float)(x * server.coef_x + server.keyboard_x / 2.0);
            y = (float)(-y * server.coef_y + server.screen_y - (server.keyboard_y / 2.0));
            data += $"{x};{y};";
        }

        if (server.IsConnected && isGestureValid && !isInputEnd)
            server.SendToClient($"u;\r\n");
        //server.SendToClient(data + "\r\n");
        hoverCounter = 0;

        isGestureValid = false;

        trRander.RemoveTrail();
    }

    /// <inheritdoc/>
    public override void GetPointerRadius(out float enterRadius, out float exitRadius)
    {
        float min_inner_angle_radians = Mathf.Deg2Rad * RETICLE_MIN_INNER_ANGLE;

        float max_inner_angle_radians =
            Mathf.Deg2Rad * (RETICLE_MIN_INNER_ANGLE + RETICLE_GROWTH_ANGLE);

        enterRadius = 2.0f * Mathf.Tan(min_inner_angle_radians);
        exitRadius = 2.0f * Mathf.Tan(max_inner_angle_radians);
    }

    /// <summary>Updates the material based on the reticle properties.</summary>
    public void UpdateDiameters()
    {

        ReticleDistanceInMeters =
      Mathf.Clamp(ReticleDistanceInMeters, RETICLE_DISTANCE_MIN, maxReticleDistance);

        if (ReticleInnerAngle < RETICLE_MIN_INNER_ANGLE)
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        }

        if (ReticleOuterAngle < RETICLE_MIN_OUTER_ANGLE)
        {
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        }

        float inner_half_angle_radians = Mathf.Deg2Rad * ReticleInnerAngle * 0.5f;
        float outer_half_angle_radians = Mathf.Deg2Rad * ReticleOuterAngle * 0.5f;

        float inner_diameter = 2.0f * Mathf.Tan(inner_half_angle_radians);
        float outer_diameter = 2.0f * Mathf.Tan(outer_half_angle_radians);

        ReticleInnerDiameter =
      Mathf.Lerp(ReticleInnerDiameter, inner_diameter, Time.unscaledDeltaTime * reticleGrowthSpeed);
        ReticleOuterDiameter =
      Mathf.Lerp(ReticleOuterDiameter, outer_diameter, Time.unscaledDeltaTime * reticleGrowthSpeed);

        if (mReticleMode == ReticleMode.Reticle)
        {
            MaterialComp.SetFloat("_InnerDiameter", ReticleInnerDiameter * ReticleDistanceInMeters);
            MaterialComp.SetFloat("_OuterDiameter", ReticleOuterDiameter * ReticleDistanceInMeters);
        }
        else if (mReticleMode == ReticleMode.StaticDot)
        {
            MaterialComp.SetFloat("_InnerDiameter", 0);
            MaterialComp.SetFloat("_OuterDiameter", StaticReticleOuterDiameter);
        }
        else
        {
            MaterialComp.SetFloat("_InnerDiameter", 0);

            outer_half_angle_radians = Mathf.Deg2Rad * ReticleOuterAngle * 0.5f;
            outer_diameter = 2.0f * Mathf.Tan(outer_half_angle_radians);
            ReticleOuterDiameter =
      Mathf.Lerp(ReticleOuterDiameter, outer_diameter, Time.unscaledDeltaTime * reticleGrowthSpeed);

            MaterialComp.SetFloat("_OuterDiameter", ReticleOuterDiameter);
        }
        MaterialComp.SetFloat("_DistanceInMeters", ReticleDistanceInMeters);
    }

    /// @cond
    /// <inheritdoc/>
    protected override void Start()
    {
        base.Start();

        if(FakePointer == null)
        {
            enabled = false;
            Debug.LogError("FakePointer doesn't set");
        }

        canvas = GameObject.Find("CanvasKeyboard");
        trLocal = canvas.transform;

        Renderer rendererComponent = GetComponent<Renderer>();
        rendererComponent.sortingOrder = reticleSortingOrder;

        MaterialComp = rendererComponent.material;
        UpdateDiameters();

        CreateReticleVertices();

    }

    /// @endcond
    /// <summary>This MonoBehavior's Awake behavior.</summary>
    private void Awake()
    {
        ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
        ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        trRander = GetComponent<TrailRender>();
        server = FindObjectOfType<Server>();
    }

    /// @cond
    /// <summary>This MonoBehavior's `Update` method.</summary>
    private void Update()
    {
        UpdateDiameters();
    }

    /// @endcond
    /// <summary>Sets the reticle pointer's target.</summary>
    /// <param name="target">The target location.</param>
    /// <param name="interactive">Whether the pointer is pointing at an interactive object.</param>
    /// <returns>Returns `true` if the target is set successfully.</returns>
    private bool SetPointerTarget(Vector3 target, bool interactive)
    {
        if (PointerTransform == null)
        {
            Debug.LogWarning("Cannot operate on a null pointer transform");
            return false;
        }

        Vector3 targetLocalPosition = PointerTransform.InverseTransformPoint(target);

        ReticleDistanceInMeters = Mathf.Clamp(targetLocalPosition.z,
                                              RETICLE_DISTANCE_MIN,
                                              maxReticleDistance);
        if (interactive)
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE + RETICLE_GROWTH_ANGLE;
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE + RETICLE_GROWTH_ANGLE;
        }
        else
        {
            ReticleInnerAngle = RETICLE_MIN_INNER_ANGLE;
            ReticleOuterAngle = RETICLE_MIN_OUTER_ANGLE;
        }

        return true;
    }

    private void CreateReticleVertices()
    {
        Mesh mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>();
        GetComponent<MeshFilter>().mesh = mesh;

        int segments_count = reticleSegments;
        int vertex_count = (segments_count + 1) * 2;

        #region Vertices

        Vector3[] vertices = new Vector3[vertex_count];

        const float kTwoPi = Mathf.PI * 2.0f;
        int vi = 0;
        for (int si = 0; si <= segments_count; ++si)
        {
            // Add two vertices for every circle segment: one at the beginning of the
            // prism, and one at the end of the prism.
            float angle = (float)si / (float)segments_count * kTwoPi;

            float x = Mathf.Sin(angle);
            float y = Mathf.Cos(angle);

            vertices[vi++] = new Vector3(x, y, 0.0f); // Outer vertex.
            vertices[vi++] = new Vector3(x, y, 1.0f); // Inner vertex.
        }
        #endregion

        #region Triangles
        int indices_count = (segments_count + 1) * 3 * 2;
        int[] indices = new int[indices_count];

        int vert = 0;
        int idx = 0;
        for (int si = 0; si < segments_count; ++si)
        {
            indices[idx++] = vert + 1;
            indices[idx++] = vert;
            indices[idx++] = vert + 2;

            indices[idx++] = vert + 1;
            indices[idx++] = vert + 2;
            indices[idx++] = vert + 3;

            vert += 2;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.triangles = indices;
        mesh.RecalculateBounds();
    }



    public override bool TriggerUp => !AirStrokeMapper.pinchIsOn;
    public override bool Triggering => AirStrokeMapper.pinchIsOn;
    public override bool TriggerDown => AirStrokeMapper.pinchIsOn;

    public enum ReticleMode
    {
        Reticle,
        StaticDot,
        DynamicDot
    }
}