using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PathConsideration", menuName = "AI/Considerations/Path")]
public class PathConsideration : Consideration
{
    //Cache pathfinding to avoid unnecessary calculations.

    private Star start = null;
    private Star goal = null;
    private List<Star> path = new List<Star>();

    public override float Calculate(Option option, Analysis analysis)
    {
        if(option is PathOption pathOption)
        {
            if(pathOption.fleet.Busy())
            {
                return 0.0f;
            }

            if(pathOption.star != goal || pathOption.fleet.star != start)
            {
                start = pathOption.fleet.star;
                goal = pathOption.star;
                path = Master.instance.PathFind(pathOption.fleet.star, pathOption.star);
            }

            return 1.0f / path.Count;
        }

        return 1.0f;
    }

    protected override Consideration CreateCopy()
    {
        return ScriptableObject.CreateInstance<PathConsideration>();
    }
}
