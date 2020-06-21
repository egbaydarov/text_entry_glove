using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DoubleClick : MonoBehaviour, IPointerDownHandler
{
    //private Image im;
    public Sprite pic;
    public Sprite picpr;
    public GameObject panel;
    private Image im;
    void Start()
    {
        im = panel.GetComponent<Image>();
        im.sprite = picpr;
    }
    public void OnPointerDown (PointerEventData eventData) 
    {
        if(eventData.clickCount == 2){
            im.sprite = pic;
            Debug.Log ("Double Click");
            eventData.clickCount = 0;
            
        }
    }    
}	