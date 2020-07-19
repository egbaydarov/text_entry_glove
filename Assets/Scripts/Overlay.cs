using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [SerializeField]
    private GameObject reticlePointer;
    
    [SerializeField]
    private GameObject LMPointer;

    [SerializeField]
    private GameObject children;

    void Start()
    {
        if (reticlePointer == null)
        {
            Debug.LogError("SoundOnHighlight: The 'reticlePointer' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (children == null)
        {
            Debug.LogError("SoundOnHighlight: The 'children' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (LMPointer == null)
        {
            Debug.LogError("SoundOnHighlight: The 'LMPointer' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

    }

    void Update()
    {
        reticlePointer.SetActive(Server.IsConnected);
        LMPointer.SetActive(Server.IsConnected);
        children.SetActive(!Server.IsConnected);
        GetComponent<MeshRenderer>().enabled = !Server.IsConnected;
    }
}
