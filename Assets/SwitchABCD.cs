using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchABCD : MonoBehaviour
{
    // Start is called before the first frame update

    SceneManagment sm;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        sm = FindObjectOfType<SceneManagment>();

    }

    public void switchA()
    {
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.ImagePlanePointing:
                gameObject.transform.localScale = new Vector3(0.835f, 0.835f, 0);
                break;
            case SceneManagment.Scenes.GestureType_v2:
                gameObject.transform.localScale = new Vector3(0.835f, 0.835f, 0);
                break;
            default:
                break;
        }
    }

    public void switchB()
    {
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.ImagePlanePointing:
                gameObject.transform.localScale = new Vector3(1.015f, 1.015f, 0);
                break;
            case SceneManagment.Scenes.GestureType_v2:
                gameObject.transform.localScale = new Vector3(1.015f, 1.015f, 0);
                break;
            default:
                break;
        }
    }

    public void switchC()
    {
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.ImagePlanePointing:
                gameObject.transform.localScale = new Vector3(1.194f, 1.194f, 0);
                break;
            case SceneManagment.Scenes.GestureType_v2:
                gameObject.transform.localScale = new Vector3(1.194f, 1.194f, 0);
                break;
            default:
                break;
        }
    }

    public void switchD()
    {
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.ImagePlanePointing:
                gameObject.transform.localScale = new Vector3(1.569f, 1.569f, 0);
                break;
            case SceneManagment.Scenes.GestureType_v2:
                gameObject.transform.localScale = new Vector3(1.569f, 1.569f, 0);
                break;
            default:
                break;
        }
    }

    public void switchTrain()
    {
        switch (sm.currentScene)
        {
            case SceneManagment.Scenes.ImagePlanePointing:
                gameObject.transform.localScale = new Vector3(1.379f, 1.379f, 0);
                break;
            case SceneManagment.Scenes.GestureType_v2:
                gameObject.transform.localScale = new Vector3(1.379f, 1.379f, 0);
                break;
            default:
                break;
        }
    }
}
