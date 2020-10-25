using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PrintKey : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Print()
    {
        if(gameObject.name == "Space")
            inputField.text += " ";
        else
        {
            inputField.text += gameObject.name;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(gameObject.name == "Space")
            inputField.text += " ";
        else
        {
            inputField.text += gameObject.name;
        }
    }
}
