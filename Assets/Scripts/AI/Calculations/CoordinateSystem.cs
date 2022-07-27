using UnityEngine;

public static partial class Calculation
{
    //used to convert between two dimensional arrays(filled one collumm at a time) to one dimensional arrays. 
    public static int TwoDimToOneDim(int x, int y, int height = 50)
    {
        return x * height + y;

    }

    public static void OneDimToTwoDim(int xy, out int x, out int y, int height = 50)
    {
        float value = (float)xy / (float)height;

        y = xy % height;
        x = Mathf.FloorToInt(value);
    }

    public static float SquareDistance(int a, int b)
    {

        Enviroment enviroment = Master.instance.enviroment;

        Star aStar = enviroment.stars[a];
        Star bStar = enviroment.stars[b];

        float x = Mathf.Abs(bStar.x - aStar.x);
        float y = Mathf.Abs(bStar.y - aStar.y);

        return (x * x) + (y * y);

    }

    public static float SquareDistance(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(b - a);

    }


}




