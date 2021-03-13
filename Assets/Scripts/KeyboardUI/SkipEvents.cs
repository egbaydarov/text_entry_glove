using UnityEngine;
using UnityEngine.EventSystems;

public class SkipEvents : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.parent != null)
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.parent != null)
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.pointerUpHandler);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.parent != null)
            ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.pointerDownHandler);
    }
}
