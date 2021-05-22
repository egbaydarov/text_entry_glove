using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetectorManager : MonoBehaviour
{
    // Start is called before the first frame update

    
    public bool isEnabled { get; set; }

    [SerializeField]
    SphereCollider gd1;
    [SerializeField]
    SphereCollider gd2;
    [SerializeField]
    SphereCollider gd3;
    [SerializeField]
    SphereCollider gd4;
    [SerializeField]
    SphereCollider gd5;
    [SerializeField]
    SphereCollider gd6;
    [SerializeField]
    SphereCollider gd7;
    [SerializeField]
    SphereCollider gd8;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isEnabled = GetComponentInParent<Transform>().gameObject.activeInHierarchy;
        gd1.isTrigger = isEnabled;
        gd2.isTrigger = isEnabled;
        gd3.isTrigger = isEnabled;
        gd4.isTrigger = isEnabled;
        gd5.isTrigger = isEnabled;
        gd6.isTrigger = isEnabled;
        gd7.isTrigger = isEnabled;
        gd8.isTrigger = isEnabled;
    }
}
