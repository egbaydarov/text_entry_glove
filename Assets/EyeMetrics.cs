using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EyeMetrics : MonoBehaviour
{
    [SerializeField]
    bool isMainKeys = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (isMainKeys)
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                Debug.Log("MainKeys Enter");
            }
        }
        else
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                Debug.Log("Predictions Enter");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (isMainKeys)
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                Debug.Log("MainKeys Exit");
            }
        }
        else
        {
            if (collision.gameObject.name.Equals("EyePointer"))
            {
                Debug.Log("Predictions Exit");
            }
        }
    }




    Rigidbody rb = null;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }
}
