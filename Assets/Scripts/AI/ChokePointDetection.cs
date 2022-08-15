using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokePointDetection
{
    static float[] congestion;

    public static void Init(Star[] stars)
    {
        float max = float.MinValue;
        congestion = new float[stars.Length];

        foreach(Star start in stars)
        {
            foreach(Star goal in stars)
            {
                if(start == goal)
                {
                    continue;
                }

                List<Star> path = Master.instance.PathFind(start, goal);

                foreach(Star star in path)
                {
                    ++congestion[star.index];
                    max = Mathf.Max(congestion[star.index], max);
                }
            }
        }

        for(int i = 0; i < congestion.Length; ++i)
        {
            congestion[i] /= max;
        }
    }

    public static float GetCongestion(Star star)
    {
        return congestion[star.index];
    }
}
