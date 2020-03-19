using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Calculation
{
    public static float TimeToMoveThroughStar(float speed = 5.0f, float starSize = 20.0f)
    {
        return starSize / speed;
    }

    public static float TimeToReachDestination(int startCoordinate, int endCoordinate, float speed = 5.0f)
    {
        int count =  Master.instance.PathFind(startCoordinate, endCoordinate).Count;
        return (count * TimeToMoveThroughStar(speed)) - (TimeToMoveThroughStar(speed) / 2);
    }

    public static float TimeToreachFaction(int startCoordinate, int endFaction, float speed = 5.0f)
    {
        List<Faction> factions = Master.instance.characters.factions;
        List<int> path = Master.instance.PathFind(startCoordinate,factions[endFaction].colonies[0].index);

        float time = 0.0f;

        for(int i = 0; i < path.Count; i++)
        {
            if(Master.instance.enviroment.stars[path[i]].factionIndex != endFaction)
            {
                time += TimeToMoveThroughStar(speed);
            }
            else
            {
                return time;
            }
        }

        return time;
    }


}
