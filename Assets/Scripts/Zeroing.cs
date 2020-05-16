using UnityEngine;
using System.Collections;
using TextEntry;

[RequireComponent(typeof(TransformInput))]
public class Zeroing : MonoBehaviour
{
    TransformInput trInput;

    private void Awake()
    {
        trInput = GetComponent<TransformInput>();
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            trInput.position_zeroing();
        }
    }
}