using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICalculatorMovementIndex
{
    int GetMovementIndex(int currentIndex, int totalPathPoints);
}
public class CalculatorNextMovementIndex : ICalculatorMovementIndex
{
    public int GetMovementIndex(int currentIndex, int totalPathPoints)
    {
        int  movementIndex = currentIndex + 1;
        if (movementIndex >= totalPathPoints)
        {
            movementIndex = 0;
        }

        return movementIndex;
    }
}

public class CalculatorPreviewMovementIndex : ICalculatorMovementIndex
{
    public int GetMovementIndex(int currentIndex, int totalPathPoints)
    {
        int movementIndex = currentIndex - 1;
        if (movementIndex < 0)
        {
            movementIndex = totalPathPoints - 1;
        }

        return movementIndex;
    }
}



