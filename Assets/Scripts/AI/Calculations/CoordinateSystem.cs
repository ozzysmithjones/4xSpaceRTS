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

        OneDimToTwoDim(enviroment.stars[a].position, out int aX, out int aY);
        OneDimToTwoDim(enviroment.stars[b].position, out int bX, out int bY);

        float x = Mathf.Abs(bX - aX);
        float y = Mathf.Abs(bY - aY);

        return (x * x) + (y * y);

    }

    public static float SquareDistance(Vector2 a, Vector2 b)
    {
        return Vector2.SqrMagnitude(b - a);

    }


}




