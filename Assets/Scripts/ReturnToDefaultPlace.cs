using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToDefaultPlace : MonoBehaviour
{
    Vector3 defaultPos;
    InteractionBehaviour ib;
    Rigidbody rb;

    float timer = 0.0f;

    void Start()
    {
        defaultPos = transform.position;
    }


    void Update()
    {
        if (transform.position != defaultPos && !ib.isGrasped)
            timer += Time.deltaTime;
        else
            timer = 0;

        if (timer % 60 >= 3)
        {
            timer = 0f;

            transform.position = defaultPos;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Awake()
    {
        ib = GetComponent<InteractionBehaviour>();
        rb = GetComponent<Rigidbody>();
    }
}
