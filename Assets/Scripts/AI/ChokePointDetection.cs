using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChokePointDetection
{
    static uint[] throughputByStar;

    public static void Init(Star[] stars)
    {
        throughputByStar = new uint[stars.Length];

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
                    ++throughputByStar[star.index];
                }
            }
        }
    }

    public static uint GetThroughput(Star star)
    {
        return throughputByStar[star.index];
    }
}
