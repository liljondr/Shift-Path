using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPart_NormalState : MovingPartState
{
    public MovingPart_NormalState(MovingPart movingPart) : base(movingPart)
    {
    }

    public override void StartState()
    {
        Debug.Log("It`s NORMAL state");
        if (movingPart.PreviousState==TypeMovingPartState.LEFT||
            movingPart.PreviousState==TypeMovingPartState.RIGHT)
        {
           Debug.Log("It`s LEFT state");
           Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
           movingPart.MoveInto(newPosition);
           Vector2 previousPosition = movingPart.GetPositionByState(movingPart.PreviousState);
           Vector2 delta = newPosition - previousPosition;
           movingPart.MoveAttachedBalls( delta);
           movingPart.DisplacementAttachedBalls(movingPart.PreviousState,TypeMovingPartState.NORMAL ); //перерозподіл приєднаних мячей

        }
    }
}
