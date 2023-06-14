using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public interface IPathID
{
   public int Id { get; }
}



[Serializable]
public class PathManager : MonoBehaviour, IPathID
{
    [SerializeField] private int id;
    public int Id
    {
        get => id;
        set => id = value;
    }

    [SerializeField] private GameObject tempVizualPrefab;
    [SerializeField] private ColorType color;
    //список початково заданих точок ( в едіторі для формування кривої-шляху)
    [SerializeField] private List<CurvePoint> listStartGivenPoints;
    

    public event Action<int, ColorType> IsPath;
    public ColorType Color => color;
    


    private float pathStep;
    private float tStep=0.005f;
    private List<Vector2> listStepCurvePoints = new List<Vector2>();
    private List<PathData> listPathPoints = new List<PathData>();
    private int drop;
    private BallItem ballPrefab;
    private Dictionary<int, BallItem> dictionaryBalls = new Dictionary<int, BallItem>();
    private bool isSpheraMove;
    private ICalculatorMovementIndex calculatorMovementIndex; //за допомогою стратегії проводимо різний підрахунок індексу для руху

    private bool isBlock;

    
   
   
        
   public void StartCalculatePath()
    {
        if (drop == 0 || ballPrefab == null)
        {
            Debug.LogError("No input data. Either drop or sphera");
            return;
        }

        pathStep = ballPrefab.Diametr / drop;
       // float lenth = CalculateLenthPathQuadraticBezierCurves(Point01.position,Point02.position,Point03.position);
       //todo перевірити чи нема повторів по іменах точок і по ID точок
       listStepCurvePoints = GetAllCurvePoints(listStartGivenPoints[0].Id,new List<CurvePoint>(listStartGivenPoints),ref listStepCurvePoints);
       listPathPoints = GetPathPoints(listStepCurvePoints);
       DebugShowPathStepLength(listPathPoints);
       foreach (PathData pathData in listPathPoints)
       {
           GameObject temp =Instantiate(tempVizualPrefab);
           temp.transform.position = pathData.Position;
       }

       IsPath?.Invoke(listPathPoints.Count, color);
    }

    

    private List<Vector2> GetAllCurvePoints(int id,List<CurvePoint> listStartGivenPoints,ref List<Vector2> listStepCurvePoints)
    {
       
        CurvePoint currentPoint = listStartGivenPoints.Find(cp => cp.Id == id);
        CurvePoint nextPoint = listStartGivenPoints.Find(cp => cp.Id == currentPoint.NextPoint.Id);
        if (currentPoint == null)
        {
            Debug.LogError($"There isn`t point with Id = {currentPoint.Id} in the listStartGivenPoints");
            return null;
        }
        
       
        
        if (currentPoint.NextType == CurvePointType.REFERENCE_Qu_BEZIER)
        {
            
            if (nextPoint == null)
            {
                Debug.LogError($"There isn`t point with Id = {nextPoint.Id} in the listStartGivenPoints");
                return null;
            }
            
            if (nextPoint.PreviewPoint.Id != currentPoint.Id || nextPoint.NextType != CurvePointType.CONTROL_Qu_BEZIER)
            {
                Debug.LogError($"There is a problem with the sequence of curve points. In particular, in the points with Id = {nextPoint.Id} and {currentPoint.Id}");
                return null;
            }
            
            CurvePoint thirdPoint = listStartGivenPoints.Find(cp => cp.Id == nextPoint.NextPoint.Id);
            if (thirdPoint == null)
            {
                //якщо остання точка, це замикаюча кривої - то присвоюємо останній точці - першу.
                if (listStartGivenPoints.Count == 2 && this.listStartGivenPoints[0].Id == nextPoint.NextPoint.Id)
                {
                    thirdPoint = this.listStartGivenPoints[0];
                }
                else
                {
                    Debug.LogError($"There isn`t point with Id = {thirdPoint.Id} in the listStartGivenPoints");
                    return null;
                }
            }
            
            if (thirdPoint.PreviewPoint.Id != nextPoint.Id )
            {
                Debug.LogError($"There is a problem with the sequence of curve points. In particular, in the points with Id = {nextPoint.Id} and {thirdPoint.Id}");
                return null;
            }

            //create poits path for BezierCurves
           List<Vector2> stepPointsFromBezierCurves = CreatorPartCurvePoints.GetCurveStepPointsFromQuadraticBezierCurves(
                currentPoint.transform.position,
                 nextPoint.transform.position,
                 thirdPoint.transform.position,
                 tStep);
           listStepCurvePoints.AddRange(stepPointsFromBezierCurves);
            
            //remove used points from list 
            listStartGivenPoints.Remove(currentPoint);
            listStartGivenPoints.Remove(nextPoint);

            if (listStartGivenPoints.Count == 0)
            {
                return listStepCurvePoints;
            }
            else
            {
                GetAllCurvePoints(thirdPoint.Id, listStartGivenPoints,ref listStepCurvePoints);
               
            }
        }

        else if (currentPoint.NextType == CurvePointType.LINE)
        {
            if (nextPoint == null)
            {
                //якщо остання точка, це замикаюча кривої - то присвоюємо останній точці - першу.
                if (listStartGivenPoints.Count == 1 && this.listStartGivenPoints[0].Id == currentPoint.NextPoint.Id)
                {
                    nextPoint = this.listStartGivenPoints[0];
                }
                else
                {
                    Debug.LogError($"There isn`t point with Id = {nextPoint.Id} in the listStartGivenPoints");
                    return null;
                }
            }
            
           if (nextPoint.PreviewPoint.Id != currentPoint.Id )
            {
                Debug.LogError($"There is a problem with the sequence of curve points. In particular, in the points with Id = {nextPoint.Id} and {currentPoint.Id}");
                return null;
            }
           
           List<Vector2> stepPointsFromLine = CreatorPartCurvePoints.GetCurveStepPointsFromLine(
               currentPoint.transform.position,
               nextPoint.transform.position,
               tStep);
           
           listStepCurvePoints.AddRange(stepPointsFromLine);
           listStartGivenPoints.Remove(currentPoint);
           
           if (listStartGivenPoints.Count == 0)
           {
               return listStepCurvePoints;
           }
           else
           {
               GetAllCurvePoints(nextPoint.Id, listStartGivenPoints,ref listStepCurvePoints);
               
           }
        }

        else
        {
            Debug.LogError($"First point must be early LINE type, or REFERENCE_Qu_BEZIER, Problem is in the points with Id =  {currentPoint.Id}");
            return null;
        }

        return listStepCurvePoints;
    }


    private float CalculateLenthPathQuadraticBezierCurves(Vector2 P0, Vector2 P1, Vector2 P2)
    {
        float x0 = P0.x;
        float y0 = P0.y;
        float x1 = P1.x;
        float y1 =  P1.y;
        float x2 = P2.x;
        float y2 =  P2.y;

        float t = 1;
        
      float  ax=x0-x1-x1+x2;
      float  ay=y0-y1-y1+y2;
      float  bx=x1+x1-x0-x0;
      float  by=y1+y1-y0-y0;
      float  A=4*((ax*ax)+(ay*ay));
      float  B=4*((ax*bx)+(ay*by));
      float  C=     (bx*bx)+(by*by);
      float  b=B/(2*A);
      float  c=C/A;
      float  u=t+b;
      float  k=c-(b*b);
      float  L=0.5f*Mathf.Sqrt(A)*
               (
                   (u*Mathf.Sqrt((u*u)+k))
                   -(b*Mathf.Sqrt((b*b)+k))
                   +(k*Mathf.Log(Mathf.Abs((u+Mathf.Sqrt((u*u)+k))/(b+Mathf.Sqrt((b*b)+k)))))
               );
      return L;

    }
    
   
    
    private List<PathData> GetPathPoints(List<Vector2> listStepCurvePoints)
    {
        List<PathData> result = new List<PathData>();
        Vector2 firstPosition = listStepCurvePoints[0];
        Direction previewDirection = Direction.None;
        Vector2 nextPosition = Vector2.zero;
        Direction nextDirection;
        int i = 1;
        float previewDistance=0;
        
        do
        {
            float currentDistance = Vector2.Distance(firstPosition, listStepCurvePoints[i]);
            if (currentDistance > pathStep)
            {
                int index = (currentDistance - pathStep < pathStep - previewDistance) ? i : i - 1;
                nextPosition= listStepCurvePoints[index];
                nextDirection = CalculateDirection.GetDirection(nextPosition-firstPosition);
                result.Add(new PathData(firstPosition, previewDirection,nextDirection));

                //Debug.Log("nextDirection = "+nextDirection);
                firstPosition = nextPosition;
                previewDirection = CalculateDirection.GetInvertDirection(nextDirection);
            }

            i++;
            previewDistance = currentDistance;
        } while (i<listStepCurvePoints.Count);
        
        Direction fromLastToFirstPoint =CalculateDirection.GetDirection(result[0].Position-nextPosition); 
        result.Add(new PathData(nextPosition, previewDirection,fromLastToFirstPoint));

        Direction fromFirstToLastPoint = CalculateDirection.GetInvertDirection(fromLastToFirstPoint);
        result[0].SetPreviewDirection(fromFirstToLastPoint);

        return result;
    }
    
   

   
    
    private void DebugShowPathStepLength(List<PathData> pathDatas)
    {
        Debug.Log($"________PathManager {color} ____________");
        for (int i = 0; i < pathDatas.Count; i++)
        {
            float distance;
            if(i+1<pathDatas.Count)
            {
                 distance = Vector2.Distance(pathDatas[i].Position, pathDatas[i + 1].Position);
                 Debug.Log($"Дистанція між {i} та {i+1} точками = {distance}");
            }
            else
            {
                distance = Vector2.Distance(pathDatas[i].Position, pathDatas[0].Position);  
                Debug.Log($"Дистанція між {i} та {0} точками = {distance}");
            }
            
        }
    }
    
   
    
    public void SetBallsDataAndCreateBalls(List<BallData> randomColorsForPathManager)
    {
        if (listPathPoints.Count != randomColorsForPathManager.Count)
        {
            Debug.LogError("The number of colors does not correspond to the number of points in the paths");
            return;
        }

        for (int i = 0; i < listPathPoints.Count; i++)
        {
            BallItem ball = Instantiate(ballPrefab);
            ball.transform.position = listPathPoints[i*drop].Position;
            ball.SetPathID(id);
            ball.SetPathIndex(i*drop);
            ball.SetID(randomColorsForPathManager[i].Id);
            ball.SetColor(randomColorsForPathManager[i].ColorType);
            dictionaryBalls[ball.ID] = ball;
            ball.OnIsDrag += OnBallIsDrag;
            ball.OnRemoveFromPath += OnBallRemoveFromPath;
            ball.gameObject.name = "Ball_" + randomColorsForPathManager[i].Id.ToString();
        }
    }

    private void OnBallIsDrag(int ballId, Direction dragDirection)
    {
        if(!isSpheraMove&&!isBlock)
        {
          
            BallItem dragBall = dictionaryBalls[ballId];
            int moveIndex;
            if (listPathPoints[dragBall.PathIndex].PreviewDiraction == dragDirection)
            {
                calculatorMovementIndex = new CalculatorPreviewMovementIndex();

            }
            else if (listPathPoints[dragBall.PathIndex].NextDiraction == dragDirection)
            {
                calculatorMovementIndex = new CalculatorNextMovementIndex();
            }
            else
            {
                return;
            }

            isSpheraMove = true;
            foreach (KeyValuePair<int, BallItem> item in dictionaryBalls)
            {
                moveIndex = calculatorMovementIndex.GetMovementIndex(item.Value.PathIndex, listPathPoints.Count);
                
                item.Value.SetDataForMove(moveIndex, listPathPoints[moveIndex], StopMoveToPoint);
            }
        }
    }
    
    private void OnBallRemoveFromPath(int ballId)
    {
        if (!dictionaryBalls.ContainsKey(ballId))
        {
            Debug.LogError($"PathManager id {Id} does not have ball id {ballId}");
            return;
        }
        BallItem ballForRemoving = dictionaryBalls[ballId];
        dictionaryBalls.Remove(ballId);
        ballForRemoving.OnIsDrag -= OnBallIsDrag;
        ballForRemoving.OnRemoveFromPath -= OnBallRemoveFromPath;
    }


    private void StopMoveToPoint()
    {
        isSpheraMove = false;
    }


    public void SetDrop(int drop)
    {
        this.drop = drop;
    }

    public void SetBallPrefab(BallItem ballItem)
    {
        ballPrefab = ballItem;
    }

    public int GetAmountPointsInPath()
    {
        return listPathPoints.Count;
    }


    public void SetBallByPathIndex(BallItem ball, int pathindex)
    {
        ball.SetPathID(id);
        ball.SetPathIndex(pathindex);
        if (dictionaryBalls.ContainsKey(ball.ID))
        {
            Debug.LogError($"The new ball id {ball.ID} that is added to the path already exists");
            return;
        }
        dictionaryBalls[ball.ID] = ball;
        ball.OnIsDrag += OnBallIsDrag;
        ball.OnRemoveFromPath += OnBallRemoveFromPath;
        
    }

    public void SetBlock(bool b)
    {
        isBlock = b;
    }

    public bool IsReadyBallsSort()
    {
        bool isReadyBallsSort = dictionaryBalls.Values.All (ball => ball.ColorType == color);
        return isReadyBallsSort;
    }
}



public enum ColorType
{
    YELLOW=0, 
    RED=1
}
