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
    Text text;

    float timer = 0.0f;


    void Start()
    {

    }


    void Update()
    {
        if (!ib.isGrasped)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            text.enabled = false;
        }

        if (timer % 60 >= 3)
        {
            timer = 0f;

            transform.localPosition = defaultPos;
            transform.localRotation = Quaternion.identity;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            text.enabled = true;
        }
    }

    private void Awake()
    {
        ib = GetComponent<InteractionBehaviour>();
        rb = GetComponent<Rigidbody>();
        text = GetComponentInChildren(typeof(Text)) as Text;
        defaultPos = transform.localPosition;
    }
}
