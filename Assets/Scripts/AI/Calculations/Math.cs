using UnityEngine;

public static partial class Calculation
{
    public static float Hypotemuse(float x, float y)
    {

        return Mathf.Sqrt((x * x) + (y * y));
    }

    public static float WrapAngle(float angle)
    {
        if (angle < 0.0f)
        {
            angle = 360.0f + angle;
        }
        if (angle > 360.0f)
        {
            angle %= 360;
        }

        return angle;

    }
}
