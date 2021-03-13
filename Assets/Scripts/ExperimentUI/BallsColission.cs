using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallsColission : MonoBehaviour
{
    public UnityEvent OnSmallBallEnter;
    public UnityEvent OnBigBallEnter;
    public UnityEvent OnMiddleSmallBallEnter;
    public UnityEvent OnMiddleBigBallEnter;
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

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.name)
        {
            case "big":
                sm.ContinueExperiment();
                break;
            case "small":
                sm.StartExperiment();
                break;
            case "middlesmall":
                sm.Exit();
                break;
            case "middlebig":
                sm.Train();
                break;
            default:
                break;
        }
    }
}
