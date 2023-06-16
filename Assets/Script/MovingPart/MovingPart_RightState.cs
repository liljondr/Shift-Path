using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_RightState : MovingPartState
{
   
    public MovingPart_RightState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s Right state");
        Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.RIGHT);
        movingPart.MoveInto(newPosition);
        Vector2 previousPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
        Vector2 delta = newPosition - previousPosition;
        movingPart.MoveAttachedBalls( delta);
        movingPart.DisplacementAttachedBalls(TypeMovingPartState.NORMAL,TypeMovingPartState.RIGHT ); //перерозподіл приєднаних мячей
    }
}
