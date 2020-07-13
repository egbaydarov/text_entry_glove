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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.name)
        {
            case "big":
                OnBigBallEnter.Invoke();
                break;
            case "small":
                OnSmallBallEnter.Invoke();
                break;
            case "middlesmall":
                OnMiddleSmallBallEnter.Invoke();
                break;
            case "middlebig":
                OnMiddleBigBallEnter.Invoke();
                break;
            default:
                break;
        }
    }
}
