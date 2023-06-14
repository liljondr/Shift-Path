using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GizmoDrawer : MonoBehaviour
{
    [SerializeField] private List<CurvePoint> listPoints;
   
    private List<int> listIdUsedPoint = new List<int>();
    float indexForBizjeCurve = 3;


    private void OnDrawGizmos()
    {
        
/*
        DrawBizjeByPoints(Point01.position, Point02.position, Point03.position, 0.05f);
        DrawBizjeByPoints(Point03.position, Point04.position, Point05.position, 0.1f);
        DrawLineByPoints(Point05.position, Point06.position, 0.25f);
        DrawBizjeByPoints(Point06.position, Point07.position, Point08.position, 0.15f);
        DrawLineByPoints(Point08.position, Point09.position, 0.1f);
        DrawBizjeByPoints(Point09.position, Point10.position, Point01.position, 0.15f);
*/
        
        listIdUsedPoint = new List<int>();
      //  SetIntoListUsedPoints(listPoints[0].Id);
        DrawStepByStep(listPoints[0]);
    }

   

    private void DrawStepByStep(CurvePoint curvePoint)
    {
        if (curvePoint.NextType == CurvePointType.REFERENCE_Qu_BEZIER)
        {
            Vector2 firstPosition = curvePoint.transform.position;
            
            CurvePoint middlePoint = curvePoint.NextPoint;
            if (middlePoint.NextType != CurvePointType.CONTROL_Qu_BEZIER)
            {
                Debug.LogError("Middle point in Bizje curve must has CurvePointType.CONTROL_Qu_BEZIER");
                return;
            }
            Vector2 middlePosition = middlePoint.transform.position;
            SetIntoListUsedPoints(middlePoint.Id);

            CurvePoint endPoint = middlePoint.NextPoint;
            Vector2 endPosition = endPoint.transform.position;
            SetIntoListUsedPoints(endPoint.Id);

            float stepForBizje = GetStepForBizje(firstPosition, middlePosition, endPosition);
            DrawBizjeByPoints(firstPosition, middlePosition, endPosition, stepForBizje);
            if (listIdUsedPoint.Count != listPoints.Count)
            {
                
                DrawStepByStep(endPoint);
            }
        }
        else if (curvePoint.NextType == CurvePointType.LINE)
        {
            Vector2 firstPosition = curvePoint.transform.position;
            
            CurvePoint endPoint = curvePoint.NextPoint;
            Vector2 endPosition = endPoint.transform.position;
            SetIntoListUsedPoints(endPoint.Id);
            float step = GetStepForLine(firstPosition, endPosition);
            DrawLineByPoints(firstPosition, endPosition, step);
            
            if (listIdUsedPoint.Count != listPoints.Count)
            {
               DrawStepByStep(endPoint);
            }
        }
        else if (curvePoint.NextType == CurvePointType.CONTROL_Qu_BEZIER)
        {
            Debug.LogError($"Point with type = CurvePointType.CONTROL_Qu_BEZIER is not the starting point of the curve segment ");
            return;
        }
    }

   

    private float GetStepForBizje(Vector2 firstPosition, Vector2 middlePosition, Vector2 endPosition)
    {
        float distance1 = Vector2.Distance(firstPosition, middlePosition);
        float distance2 = Vector2.Distance(middlePosition, endPosition);
        float distance = distance1 + distance2;

        float step = indexForBizjeCurve / distance;
        return step;
    }
    
    private float GetStepForLine(Vector2 firstPosition, Vector2 endPosition)
    {
        float distance = Vector2.Distance(firstPosition, endPosition);
        float step = indexForBizjeCurve / distance;
        return step;
    }

    private void SetIntoListUsedPoints(int i)
    {
        if (listIdUsedPoint.Contains(i))
        {
           Debug.LogError($"Point with id = {i} already is in The list");
            return;
        }
        listIdUsedPoint.Add(i);
    }


    private void DrawBizjeByPoints(Vector2 firstPoint,Vector2 middlePoint, Vector2 endPoint,float step)
    {
        Gizmos.color = Color.white;
        for (float t = 0; t < 1; t += step)
        {
            Vector3 newPosition = Mathf.Pow((1 - t), 2) * firstPoint + 2 * t * (1 - t) * middlePoint +
                                  Mathf.Pow(t, 2) * endPoint;

            Gizmos.DrawSphere(newPosition, 0.5f);
        }
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(firstPoint, middlePoint);
        Gizmos.DrawLine(middlePoint, endPoint);
    }
    
    private void DrawLineByPoints(Vector3 firstPoint, Vector3 endPoint, float step)
    {
        Gizmos.color = Color.white;
        for (float t = 0; t < 1; t += step)
        {
            Vector3 newPosition = (1-t)*firstPoint+t*endPoint;
            Gizmos.DrawSphere(newPosition, 0.5f);
        }
    }
}
