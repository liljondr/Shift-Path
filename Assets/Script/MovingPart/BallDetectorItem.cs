using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BallDetectorItem : MonoBehaviour
{
    [SerializeField] private TypeMovingPartState state;
    public TypeMovingPartState State => state;

    public int PathId => (int) startPathId;
    public int PathIndex => (int)startPathIndex;

    private BallItem ball;
    private int? startPathId;
    private int? startPathIndex;
    
   private void OnTriggerEnter(Collider collider)
       {
           if (collider.gameObject.tag == "Ball")
           {
               BallItem ballItem = collider.GetComponent<BallItem>();
               if (ballItem != null)
               {
                   ball = ballItem;
                   if(startPathId==null)
                   {
                       startPathId = ballItem.PathID;
                   }
                   if(startPathIndex==null)
                   {
                       startPathIndex = ballItem.PathIndex;
                   }
               }
           }
       }

   public void MoveAttachedBalls(Vector2 delta, float speed)
   {
       ball.transform.DOLocalMove((Vector2)ball.transform.position + delta, speed);
   }

   public void BlockBall()
   {
       ball.SwitchBlock(true);
       ball.OnRemoveFromPath?.Invoke(ball.ID);
   }

   public void RemoveBallByData(int pathId, int pathIndex)
   {
       ball.OnRemoveFromPath?.Invoke(ball.ID);
       //мяч який готовий для переміщення переносимо в новий шлях
       GlobalSignals.OnDisplacementBallIntoNewPath?.Invoke(ball,pathId, pathIndex);
      
   }

   public void UnBlockBall()
   {
       ball.SwitchBlock(true);
       //розміщення кульки на місце до вона була раніше
       GlobalSignals.OnDisplacementBallIntoNewPath?.Invoke(ball,(int)startPathId, (int)startPathIndex);
   }

   public void ReturnAtStartPlace()
   {
       //видалаємо звідти де він був раніше 
       ball.OnRemoveFromPath?.Invoke(ball.ID);
       //розміщення кульки на місце до вона була раніше
       GlobalSignals.OnDisplacementBallIntoNewPath?.Invoke(ball,(int)startPathId, (int)startPathIndex);
       
   }
}
