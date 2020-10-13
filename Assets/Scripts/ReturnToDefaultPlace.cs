using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReturnToDefaultPlace : MonoBehaviour
{
    Vector3 defaultPos;
    InteractionBehaviour ib;
    Rigidbody rb;

    void Update()
    {
        if (transform.position.y < -5 || transform.position.y > 3)
        {
            transform.localPosition = defaultPos;
            transform.localRotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Awake()
    {
        ib = GetComponent<InteractionBehaviour>();
        rb = GetComponent<Rigidbody>();
        defaultPos = transform.localPosition;
    }
}
