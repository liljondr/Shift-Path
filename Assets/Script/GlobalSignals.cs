using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSignals : MonoBehaviour
{
    public delegate void Delegat_DisplacementBallIntoNewPath (BallItem ball,int pathId, int pathIndex);
    public static Delegat_DisplacementBallIntoNewPath OnDisplacementBallIntoNewPath; 
}
