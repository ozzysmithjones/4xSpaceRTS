using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Calculation
{
    public static float Hypotemuse(float x, float y)
    {

        return Mathf.Sqrt((x * x) + (y * y));
    }

}
