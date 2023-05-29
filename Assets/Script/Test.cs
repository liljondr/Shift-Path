using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public void OnMouseDrag()
           {
             //  Debug.Log("OnMouseDrag");
           }
           
    public void OnMouseDown()
           {
             //  Debug.Log("OnMouseDown");
           }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag  delta = "+eventData.delta);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
    }
}
