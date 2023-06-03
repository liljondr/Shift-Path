using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingPartForTapDetect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TypeMovingPartState partState;
    public TypeMovingPartState PartState => partState;

    public event Action<TypeMovingPartState> OnClick;

    

    public void OnPointerClick(PointerEventData eventData)
    {
       OnClick?.Invoke(partState);
    }
}

public enum TypeMovingPartState
{
    NORMAL,
    LEFT,
    RIGHT
}
