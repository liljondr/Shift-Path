using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.Mover;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using Object = System.Object;


public class MovingPart : MonoBehaviour
{

   [SerializeField] private Transform movingView;
   [Space]
   [SerializeField] private List<MovingPartData> listData;
   [Space]
   [SerializeField] private List<BallDetector> listBallDetector;
   
  

   private MovingPartState state;
   private TypeMovingPartState carrentState;
   public readonly float speedMove = 0.5f;
   private Dictionary<TypeMovingPartState,MovingPartData> dictonaryDataByState= new Dictionary<TypeMovingPartState, MovingPartData>();
   // [SerializeField] private List<Transform> listPosition;
   //  [SerializeField] private List<MovingPartForTapDetect> listTabDetected;
   //  [SerializeField] private Transform view;


   private void Start()
   {
      CreateDictionaryData();
      SubscriptionToTap();
      MovingPartData dataForNormal = GetDataByState(TypeMovingPartState.NORMAL);
      movingView.localPosition = dataForNormal.Position.localPosition;
     

      carrentState = TypeMovingPartState.NORMAL;
      state = new MovingPart_NormalState(this);
      state.StartState();

   }


   private void CreateDictionaryData()
   {
      foreach (MovingPartData data in listData)
      {
         dictonaryDataByState[data.State] = data;
      }
   }
   
   private void SubscriptionToTap()
   {
      foreach (MovingPartData data in listData)
      {
         if(data.PartForDetectTab!=null)
         {
            data.PartForDetectTab.OnClick += OnTapDetected;
         }
      }
   }

   

   private MovingPartData GetDataByState(TypeMovingPartState typeState)
   {
    
    if (!dictonaryDataByState.ContainsKey(typeState))
    {
       Debug.LogError($"Moving part doesn`t have data for {typeState} state");
       return null;
    }
    return dictonaryDataByState[typeState];
   }
   
   private void OnTapDetected(TypeMovingPartState typestate)
   {
      if (typestate == TypeMovingPartState.LEFT)
      {
         if (carrentState == TypeMovingPartState.NORMAL)
         {
            state = new MovingPart_LefrState(this);
            carrentState = TypeMovingPartState.LEFT;
           
         }
         else if (carrentState == TypeMovingPartState.RIGHT)
         {
            state = new MovingPart_NormalState(this);
            carrentState = TypeMovingPartState.NORMAL;
         }
         
         state.StartState();
      }
      
      else if(typestate ==TypeMovingPartState.RIGHT)
      {
         if (carrentState == TypeMovingPartState.NORMAL)
         {
            state = new MovingPart_RightState(this);
            carrentState = TypeMovingPartState.RIGHT;
         }
         else if (carrentState == TypeMovingPartState.LEFT)
         {
            state = new MovingPart_NormalState(this);
            carrentState = TypeMovingPartState.NORMAL;
         }
         
         state.StartState();
      }

   }

   public bool IsInNormalPosition()
   {
      MovingPartData dataForNormal = GetDataByState(TypeMovingPartState.NORMAL);
      bool isInNormalPoition = (movingView.localPosition.x == dataForNormal.Position.localPosition.x &&
                                movingView.localPosition.y == dataForNormal.Position.localPosition.y)
         ? true
         : false;
      return isInNormalPoition;
   }

   public Vector2 GetPositionByState(TypeMovingPartState typeState)
   {
      MovingPartData movingPartData = GetDataByState(typeState);

      return movingPartData.Position.localPosition;
     ;
   }

   public void MoveInto(Vector2 newPosition)
   {
      movingView.DOLocalMove(newPosition, speedMove);
   }
}

[Serializable]
public class MovingPartData
{
   [SerializeField] private TypeMovingPartState state;
   [SerializeField] private Transform position;
   [SerializeField] private MovingPartForTapDetect partForDetectTab;
   [SerializeField] private PathManager pathManager;
 //  private Queue<BallItem> queueBalls = new Queue<BallItem>();

   public TypeMovingPartState State => state;

   public Transform Position => position;

   public MovingPartForTapDetect PartForDetectTab => partForDetectTab;

   public IPathID PathId => pathManager;


}
