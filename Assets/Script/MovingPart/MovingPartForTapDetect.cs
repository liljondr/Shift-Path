using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingPartForTapDetect : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TypeMovingPartState partState;
    public TypeMovingPartState PartState => partState;
    [SerializeField] private PathManager pathManager;
    public IPathID PathId => pathManager;

    public event Action<TypeMovingPartState> OnClick;

    

    public void OnPointerClick(PointerEventData eventData)
    {
       OnClick?.Invoke(partState);
    }
}