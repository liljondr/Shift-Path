using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingPartState 
{
   protected MovingPart movingPart;

   public MovingPartState(MovingPart movingPart)
   {
      this.movingPart = movingPart;
   }

   public abstract void StartState();
}
