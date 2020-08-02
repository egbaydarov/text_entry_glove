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

    Server server;

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
    private void Awake()
    {
        server = FindObjectOfType<Server>();
    }

    void Update()
    {
        reticlePointer.SetActive(server.IsConnected);
        LMPointer.SetActive(server.IsConnected);
        children.SetActive(!server.IsConnected);
        GetComponent<MeshRenderer>().enabled = !server.IsConnected;
    }
}
