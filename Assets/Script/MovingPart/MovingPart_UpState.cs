using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_UpState : MovingPartState
{
    public MovingPart_UpState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s Up state");
        Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.UP);
        movingPart.MoveInto(newPosition);
        Vector2 previousPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
        Vector2 delta = newPosition - previousPosition;
        movingPart.MoveAttachedBalls( delta);
        movingPart.DisplacementAttachedBalls(TypeMovingPartState.NORMAL,TypeMovingPartState.UP ); //перерозподіл приєднаних мячей

    }
}
