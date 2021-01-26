using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchABCD : MonoBehaviour
{
    // Start is called before the first frame update

    SceneManagment sm;

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

    const float scale_coef = 1.972f;

    GameObject ScaledObject;
    GameObject ScaledObjectPrediction;
    public static string CurrentMode = "A";

    Text TextAbove;

    void Start()
    {
        if(SceneManagment.isMain)
        {
            switchA();
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

        ScaledObjectPrediction = GameObject.Find("Prediction");

        GameObject KeyboardHolder = GameObject.Find("Keyboard Holder");
        KeyboardHolder.transform.localScale = new Vector3(scale_coef, scale_coef, 1);

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
        CurrentMode = "A";
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[0], ScaleValues[0], 1);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[0], ScaleValues[0], 1);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[0], ScaleValues1[0], 1);
                ScaledObjectPrediction.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 840);

                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchB()
    {
        CurrentMode = "B";
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[1], ScaleValues[1], 1);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[1], ScaleValues1[1], 1);
                ScaledObjectPrediction.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 840);

                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchC()
    {
        CurrentMode = "C";

        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[2], ScaleValues[2], 1);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[2], ScaleValues[2], 1);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[2], ScaleValues1[2], 1);
                ScaledObjectPrediction.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 840);

                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchD()
    {
        CurrentMode = "D";

        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[3], ScaleValues[3], 1);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[3], ScaleValues[3], 1);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[3], ScaleValues1[3], 1);
                ScaledObjectPrediction.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 760);
                break;
            default:
                break;
        }
        UpdateText();
    }

    public void switchTrain()
    {
        CurrentMode = "Train";

        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.IMAGE_PLANE_POINTING:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[4], ScaleValues[4], 1);
                break;
            case SceneManagment.Scenes.GESTURE_TYPE:
                ScaledObject.transform.localScale = new Vector3(ScaleValues[4], ScaleValues[4], 1);
                break;
            case SceneManagment.Scenes.EYE_GAZE_AND_COMMIT:
                ScaledObject.transform.localScale = new Vector3(ScaleValues1[4], ScaleValues1[4], 1);
                ScaledObjectPrediction.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 840);
                break;
            default:
                break;
        }
        UpdateText();
    }
}
