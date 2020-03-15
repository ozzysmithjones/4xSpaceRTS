using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Calculation
{
    //used to convert between two dimensional arrays(filled one collumm at a time) to one dimensional arrays. 
    public static int TwoDimToOneDim(int x, int y, int height = 50)
    {
        return x * height + y;

    }

    public static void OneDimToTwoDim(int xy,out int x, out int y, int height = 50)
    {
        float value = (float)xy / (float)height;

        y = xy % height;
        x = Mathf.FloorToInt(value);
    }


}




