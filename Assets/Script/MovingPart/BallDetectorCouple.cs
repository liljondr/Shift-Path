using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;


public class BallDetectorCouple : MonoBehaviour
{
    [SerializeField] private List<BallDetectorItem> listBallDetectors;
   
    public void MoveAttachedBalls(Vector2 delta, float speed)
    {
        foreach (BallDetectorItem ballDetector in listBallDetectors)
        {
            ballDetector.MoveAttachedBalls(delta, speed);
           
        }
    }
    
    public void DisplacementBallIntoNewMode(TypeMovingPartState newType)
    {
        BallDetectorItem ballDetectorItemForBlock = listBallDetectors.Find(item => item.State == newType);
        if (ballDetectorItemForBlock == null)
        {
            Debug.LogError("Didn`t find BallDetectorItem by type = "+newType);
            return;
        }
        ballDetectorItemForBlock.BlockBall();
        
        BallDetectorItem ballDetectorItemForRemove = listBallDetectors.Find(item => item.State != newType);
        if (ballDetectorItemForRemove == null)
        {
            Debug.LogError("Didn`t find BallDetectorItem by type != "+newType);
            return;
        }

        ballDetectorItemForRemove.RemoveBallByData(ballDetectorItemForBlock.PathId, ballDetectorItemForBlock.PathIndex);
    }

   
    public void ReturnMovingPartToStartPosition(TypeMovingPartState previousType)
    {
        BallDetectorItem ballDetectorItemForUnBlock = listBallDetectors.Find(item => item.State == previousType);
        if (ballDetectorItemForUnBlock == null)
        {
            Debug.LogError("Didn`t find BallDetectorItem by type = "+previousType);
            return;
        }
        ballDetectorItemForUnBlock.UnBlockBall();
        
        BallDetectorItem ballDetectorItemForReturn = listBallDetectors.Find(item => item.State != previousType);
        if (ballDetectorItemForReturn == null)
        {
            Debug.LogError("Didn`t find BallDetectorItem by type != "+previousType);
            return;
        }

        ballDetectorItemForReturn.ReturnAtStartPlace();

    }


    
}
