using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_LefrState : MovingPartState
{
   

    public MovingPart_LefrState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s LEFT state");
        Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.LEFT);
        movingPart.MoveInto(newPosition);
    }
}
