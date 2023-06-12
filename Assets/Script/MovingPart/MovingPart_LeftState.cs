using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_LeftState : MovingPartState
{
   

    public MovingPart_LeftState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s LEFT state");
        Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.LEFT);
        movingPart.MoveInto(newPosition);
        Vector2 previousPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
        Vector2 delta = newPosition - previousPosition;
        movingPart.MoveAttachedBalls( delta);
        movingPart.DisplacementAttachedBalls(TypeMovingPartState.NORMAL,TypeMovingPartState.LEFT ); //перерозподіл приєднаних мячей
    }
}
