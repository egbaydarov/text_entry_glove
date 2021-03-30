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

    public bool hideOverlayFlag = true;

    void Start()
    {
        EntryProcessing entryProc = FindObjectOfType<EntryProcessing>();

        if (!SceneManagment.isMain)
            hideOverlayFlag = false;


        if (reticlePointer == null)
        {
            Debug.LogError("Overlay: The 'reticlePointer' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (children == null)
        {
            Debug.LogError("Overlay: The 'children' field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (LMPointer == null)
        {
            Debug.LogError("Overlay: The 'LMPointer' field cannot be left unassigned. Disabling the script");
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
        reticlePointer.SetActive(server.IsConnected && hideOverlayFlag);
        LMPointer.SetActive(server.IsConnected && hideOverlayFlag);

        children.SetActive(!server.IsConnected || !hideOverlayFlag);
        GetComponent<MeshRenderer>().enabled = !server.IsConnected || !hideOverlayFlag;
    }
}
