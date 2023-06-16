using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_DownState : MovingPartState

{
    public MovingPart_DownState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s Down state");
        Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.DOWN);
        movingPart.MoveInto(newPosition);
        Vector2 previousPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
        Vector2 delta = newPosition - previousPosition;
        movingPart.MoveAttachedBalls( delta);
        movingPart.DisplacementAttachedBalls(TypeMovingPartState.NORMAL,TypeMovingPartState.DOWN ); //перерозподіл приєднаних мячей

    }
}
