using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathData : MonoBehaviour
{
    private Direction previewDiraction;
    public Direction PreviewDiraction => previewDiraction;

    private Direction nextDiraction;
    public Direction NextDiraction => nextDiraction;

    private Vector2 position;
    public Vector2 Position => position;

    public PathData(Vector2 position, Direction previewDiraction, Direction nextDiraction)
    {
        this.position = position;
        this.previewDiraction = previewDiraction;
        this.nextDiraction = nextDiraction;
    }

    public void SetPreviewDirection(Direction previewDiraction)
    {
        this.previewDiraction = previewDiraction;
    }
}

