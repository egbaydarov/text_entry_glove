using LeapMotionGesture;
using UnityEngine;
using UnityEngine.UI;

public class SwitchABCD : MonoBehaviour
{

    SceneManagment sm;

    bool hgacflag = true;

    private float[] ScaleValues = {0.835f / scale_coef,
        1.015f / scale_coef,
        1.194f / scale_coef,
        1.569f / scale_coef,
        1.379f / scale_coef };

    private float[] ScaleValues1 = {1.379f / scale_coef,
        1.569f / scale_coef,
        1.766f / scale_coef,
        1.972f / scale_coef,
        1.667f / scale_coef };

    private float[] ScaleValues2 = { 0.221f, 0.251f, 0.283f, 0.315f, 0.350f };

    private float[] ScaleValues3 = { 3f, 4f, 5f, 6f, 4.5f };

    private Vector3[] ScaleValues4 = { new Vector3(0.15f, -0.3f, -0.55f), new Vector3(0.15f, -0.3f, -0.25f), new Vector3(0.15f, -0.3f, -0.15f), new Vector3(0.15f, -0.3f, 0f), new Vector3(0.15f, -0.3f, -0.35f) };

    const float scale_coef = 1f;

    GameObject ScaledObject;
    public static string CurrentMode = "A";
    TrailRender tr;

    PointerHandler pointerHandler;

    AirStrokeMapper airStrokeMapper;

    Text TextAbove;

    void Start()
    {
        if (SceneManagment.isMain)
        {
            switchMain();
        }
        else
        {
            switchTrain();
        }
    }

    void UpdateText()
    {
        TextAbove.text = $"Режим\n{CurrentMode}";
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        sm = FindObjectOfType<SceneManagment>();
        SwitchABCD go = FindObjectOfType<SwitchABCD>();

        GameObject goText = GameObject.Find("Mode");
        TextAbove = goText.GetComponent<Text>();

        tr = FindObjectOfType<TrailRender>();

        airStrokeMapper = FindObjectOfType<AirStrokeMapper>();

        GameObject KeyboardHolder = GameObject.Find("Keyboard Holder");
        KeyboardHolder.transform.localScale = new Vector3(scale_coef, scale_coef, 1);

        pointerHandler = FindObjectOfType<PointerHandler>();

        if (go == null)
        {
            enabled = false;
            Debug.Log("Can't find SwitchABCD object.");
            return;
        }
        else
        {
            ScaledObject = go.gameObject;
        }
    }

    public void switchA()
    {
        int i = 0;
        CurrentMode = "A";
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[0], ScaleValues[0], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[0]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[0], ScaleValues[0], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[0]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                airStrokeMapper.fixedValue = ScaleValues3[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[0]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[0], ScaleValues2[0], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[0]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                if (hgacflag)
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[i], ScaleValues[i], 1);
                    tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[i]);
                }
                else
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                    airStrokeMapper.fixedValue = ScaleValues3[i];
                }
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchB()
    {
        CurrentMode = "B";
        int i = 1;
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                airStrokeMapper.fixedValue = ScaleValues3[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[0]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[i], ScaleValues2[i], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[i]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                if (hgacflag)
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[i], ScaleValues[i], 1);
                    tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[i]);
                }
                else
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                    airStrokeMapper.fixedValue = ScaleValues3[i];
                }
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchC()
    {
        CurrentMode = "C";
        int i = 2;
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[2], ScaleValues[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[2]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[2], ScaleValues[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[2]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                airStrokeMapper.fixedValue = ScaleValues3[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[0]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[i], ScaleValues2[i], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[i]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                if (hgacflag)
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[i], ScaleValues[i], 1);
                    tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[i]);
                }
                else
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                    airStrokeMapper.fixedValue = ScaleValues3[i];
                }
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchD()
    {
        CurrentMode = "D";
        int i = 3;
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[3], ScaleValues[3], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[3]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[3], ScaleValues[3], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[3]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                airStrokeMapper.fixedValue = ScaleValues3[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[0]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[i], ScaleValues2[i], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[i]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                if (hgacflag)
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[i], ScaleValues[i], 1);
                    tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[i]);
                }
                else
                {
                    ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                    airStrokeMapper.fixedValue = ScaleValues3[i];
                }
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchTrain()
    {
        CurrentMode = "Train";
        int i = 1;
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[2]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[i], ScaleValues2[i], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[i]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                airStrokeMapper.fixedValue = ScaleValues3[2];
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchMain()
    {
        CurrentMode = "Main";
        int i = 1;
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues[1]);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues1[2]);
                break;
            case SceneManagment.Scenes.ARTICULATED_HANDS:
                ScaledObject.transform.localScale = new Vector3(ScaleValues2[i], ScaleValues2[i], 1);
                tr.setLineWidth(tr.LINE_WIDTH * ScaleValues2[i]);
                break;
            case SceneManagment.Scenes.HEAD_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                airStrokeMapper.fixedValue = ScaleValues3[2];
                break;
            case SceneManagment.Scenes.OCULUS_QUEST:
                pointerHandler.shoulderOffset = ScaleValues4[i];
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            default:
                break;
        }
        UpdateText();
    }
}
