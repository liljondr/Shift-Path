using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateDirection : MonoBehaviour
{
    public static   Direction GetDirectionOf8(Vector2 vector)
    {
        Direction direction= Direction.None;
        float angle = GetAngleTo0X( vector);
        if ((angle < 22.5f &&  angle >= -22.5f))
        {
            direction = Direction.Right;
        }
        else if (angle < 67.5f && angle >= 22.5f)
        {
            direction = Direction.UpRight;
        }
        else if (angle < 112.5f && angle >= 67.5f)
        {
            direction = Direction.Up;
        }
        else if (angle < 157.5f && angle >= 112.5f)
        {
            direction = Direction.UpLeft;
        } 
        else if ((angle <= 180 && angle >= 157.5f)||(angle >=-180  && angle < -157.5f))
        {
            direction = Direction.Left;
        }
        else if (angle < -112.5f && angle >= -157.5f)
        {
            direction = Direction.DownLeft;
        }
        else if (angle < -67.5f && angle >= -112.5f)
        {
            direction = Direction.Down;
        }
        else if (angle < -22.5f && angle >= -67.5f)
        {
            direction = Direction.DownRight;
        }

        return direction;
    }
    
    public static   Direction GetDirectionOf4(Vector2 vector)
    {
        Direction direction= Direction.None;
        float angle = GetAngleTo0X( vector);
        if ((angle < 45f &&  angle >= -45f))
        {
            direction = Direction.Right;
        }
       
        else if (angle < 135f && angle >= 45f)
        {
            direction = Direction.Up;
        }
        
        else if ((angle <= 180 && angle >= 135f)||(angle >=-180  && angle < -135f))
        {
            direction = Direction.Left;
        }
        
        else if (angle < -45f && angle >= -135f)
        {
            direction = Direction.Down;
        }
       

        return direction;
    }
    
    private static float GetAngleTo0X(Vector2 firstPosition)
    {
        double  angle = Math.Atan2(firstPosition.y, firstPosition.x);
        
        
        float  resultAngle =(float) angle * (180 / (float)Math.PI);
        return resultAngle;
    }
    
    public static Direction GetInvertDirection(Direction direction)
    {
        Direction invertDirection = Direction.None;
        switch (direction)
        {
            case Direction.Right:
                invertDirection = Direction.Left;
                break;
            case Direction.UpRight:
                invertDirection = Direction.DownLeft;
                break;
            case Direction.Up:
                invertDirection = Direction.Down;
                break;
            case Direction.UpLeft:
                invertDirection = Direction.DownRight;
                break;
            case Direction.Left:
                invertDirection = Direction.Right;
                break;
            case Direction.DownLeft:
                invertDirection = Direction.UpRight;
                break;
            case Direction.Down:
                invertDirection = Direction.Up;
                break;
            case Direction.DownRight:
                invertDirection = Direction.UpLeft;
                break;
        }

        return invertDirection;
    }
}

public enum Direction
{
    None,
    Up,
    Left,
    Right,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

