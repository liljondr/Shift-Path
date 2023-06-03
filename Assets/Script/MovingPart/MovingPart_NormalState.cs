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
        if (!movingPart.IsInNormalPosition())
        {
           Vector2 newPosition = movingPart.GetPositionByState(TypeMovingPartState.NORMAL);
           movingPart.MoveInto(newPosition);
        }
    }
}
