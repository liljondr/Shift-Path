using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class CreatorPartCurvePoints : MonoBehaviour
{
    public static List<Vector2> GetCurveStepPointsFromQuadraticBezierCurves(Vector2 firstPoint, Vector2 middlePoint, Vector2 endPoint, float tStep)
    {
        List<Vector2> result = new List<Vector2>();
        float t ;
        for (t = 0; t <=1; t+=tStep)
        {
            Vector2 tPoint =  Mathf.Pow((1 - t), 2) * firstPoint + 2 * t*(1 - t) * middlePoint + t * t * endPoint;
           
            result.Add(tPoint);
        }

        return result;
    }

    public static List<Vector2> GetCurveStepPointsFromLine(Vector3 firstPoint, Vector3 endPoint, float tStep)
    {
        List<Vector2> result = new List<Vector2>();
        float t ;
        for (t = 0; t <=1; t+=tStep)
        {
            Vector2 tPoint =  Vector2.Lerp(firstPoint, endPoint,t);
           
            result.Add(tPoint);
        }

        return result;
    }
}
