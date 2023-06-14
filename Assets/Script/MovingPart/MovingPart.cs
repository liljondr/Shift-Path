using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using Object = System.Object;


public class MovingPart : MonoBehaviour
{

   [SerializeField] private Transform movingView;
   [Space] [SerializeField] private List<MovingPosition> listPosition;
   [Space]
   [SerializeField] private List<MovingPartForTapDetect> listTapDetector;
   [Space]
   [SerializeField] private List<BallDetectorCouple> listBallDetector;
   
  

   private MovingPartState state;
   private TypeMovingPartState currentState;
   public TypeMovingPartState CurrentState => currentState;
   private TypeMovingPartState previousState;
   public TypeMovingPartState PreviousState => previousState;
   public readonly float speedMove = 0.5f;
   public event Action< bool,List<IPathID>> SwitchBlockPath;
   public event Action<List<IPathID>> OnCheckSortedBalls;
   
 private Dictionary<TypeMovingPartState, MovingPartState> stateDictionary =
      new Dictionary<TypeMovingPartState, MovingPartState>();


   private void Start()
   {
    CreateStateDictionary();
      SubscriptionToTap();
    SettingStartNormalState();
      

   }

  


 
   
   private void CreateStateDictionary()
   {
     stateDictionary[TypeMovingPartState.NORMAL] = new MovingPart_NormalState(this);
     stateDictionary[TypeMovingPartState.LEFT] = new MovingPart_LeftState(this);
     stateDictionary[TypeMovingPartState.RIGHT] = new MovingPart_RightState(this);
   }
   
   private void SubscriptionToTap()
   {
      foreach (MovingPartForTapDetect tapDetector in listTapDetector)
      {
         tapDetector.OnClick += OnTapDetected;
      }
   }
   
 
   
   private void SettingStartNormalState()
   {
      Vector2 normalPosition =GetPositionByState(TypeMovingPartState.NORMAL);
      
      movingView.localPosition = normalPosition;

      currentState = TypeMovingPartState.NORMAL;
      state = stateDictionary[TypeMovingPartState.NORMAL];
      state.StartState();
   }



   private void OnTapDetected(TypeMovingPartState typestate)
   {
      if (typestate == TypeMovingPartState.LEFT)
      {
         if (currentState == TypeMovingPartState.NORMAL)
         {
            currentState = TypeMovingPartState.LEFT;
            previousState = TypeMovingPartState.NORMAL;
            state = stateDictionary[currentState];
            state.StartState();

         }
         else if (currentState == TypeMovingPartState.RIGHT)
         {
            currentState = TypeMovingPartState.NORMAL;
            previousState = TypeMovingPartState.RIGHT;
            state = stateDictionary[currentState];
            state.StartState();
         }
         
         
      }
      
      else if(typestate ==TypeMovingPartState.RIGHT)
      {
         if (currentState == TypeMovingPartState.NORMAL)
         {
            currentState = TypeMovingPartState.RIGHT;
            previousState = TypeMovingPartState.NORMAL;
            state = stateDictionary[currentState];
            state.StartState();
         }
         else if (currentState == TypeMovingPartState.LEFT)
         {
            currentState = TypeMovingPartState.NORMAL;
            previousState = TypeMovingPartState.LEFT;
            state = stateDictionary[currentState];
            state.StartState();
         }
         
         
      }

   }

  

   public Vector2 GetPositionByState(TypeMovingPartState typeState)
   {
      MovingPosition movingPosition = listPosition.Find(pos=>pos.State==typeState);
      if (movingPosition == null)
      {
         Debug.LogError($"Didn`t find position for {typeState} State");
        return Vector2.zero ;
      }
      return movingPosition.Position;
     ;
   }

   public void MoveInto(Vector2 newPosition)
   {
      movingView.DOLocalMove(newPosition, speedMove);
   }

   public void MoveAttachedBalls(Vector2 delta)
   {
      foreach (BallDetectorCouple detector in listBallDetector)
      {
         detector.MoveAttachedBalls(delta,speedMove);
      }
   }

   public void DisplacementAttachedBalls(TypeMovingPartState previousType, TypeMovingPartState newType)
   {
      if (newType != TypeMovingPartState.NORMAL)
      {
         foreach (BallDetectorCouple ballDetector in listBallDetector)
         {
            ballDetector.DisplacementBallIntoNewMode(newType);
         }

         List<IPathID> listPathIdForBlock =listTapDetector.Where(td => td.PartState != newType).Select(td=>td.PathId).ToList();
         SwitchBlockPath?.Invoke(true,listPathIdForBlock);
         
      }
      else
      {
         foreach (BallDetectorCouple ballDetector in listBallDetector)
         {
            ballDetector.ReturnMovingPartToStartPosition(previousType);
         }
         //розблокуємо усі шляхи
         List<IPathID> listPathId =listTapDetector.Select(td=>td.PathId).ToList();
         SwitchBlockPath?.Invoke(false,listPathId);
         OnCheckSortedBalls?.Invoke(listPathId);
      }
   }
}


