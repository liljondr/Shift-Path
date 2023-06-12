using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPosition : MonoBehaviour
{
  [SerializeField] private TypeMovingPartState state;
  public TypeMovingPartState State => state;
  public Vector2 Position => transform.localPosition;
}
