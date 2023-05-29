using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GizmoDrawer : MonoBehaviour
{

    public Transform Point01;
    public Transform Point02;
    public Transform Point03;
    public Transform Point04;
    public Transform Point05;
    public Transform Point06;
    public Transform Point07;
    public Transform Point08;
    public Transform Point09;
    public Transform Point10;
    private float startTime;
    
    private float totalDistance;

    private List<Vector2> listPosition = new List<Vector2>();

   

    private void OnDrawGizmos()
    {
        float step =0.05f;

        DrawBizjeByPoints(Point01.position, Point02.position, Point03.position,0.05f);
        DrawBizjeByPoints(Point03.position, Point04.position, Point05.position,0.1f);
        DrawLineByPoints(Point05.position, Point06.position, 0.25f);
        DrawBizjeByPoints(Point06.position, Point07.position, Point08.position,0.15f);
        DrawLineByPoints(Point08.position, Point09.position, 0.1f);
        DrawBizjeByPoints(Point09.position, Point10.position, Point01.position,0.15f);
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
