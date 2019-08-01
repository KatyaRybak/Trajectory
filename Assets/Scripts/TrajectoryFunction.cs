using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Trajectory
{
    Linear,
    Sinusoid,
    Parabola

}

public class TrajectoryFunction: ScriptableObject
{
    public static Vector2 SinusMove(float x)
    {
        float y = Mathf.Sin(x) * 0.2f;
        Vector2 newPlanePosition = new Vector2(x, y);
        return newPlanePosition;
    }

    public static Vector2 LineMove(float x)
    {
        float y = x / 5;
        Vector2 newPlanePosition = new Vector2(x, y);
        return newPlanePosition;
    }

    public static Vector2 ParabolaMove(float x)
    {
        float y = -x * x / 20 ;
        Vector2 newPlanePosition = new Vector2(x, y);
        return newPlanePosition;
    }

}
