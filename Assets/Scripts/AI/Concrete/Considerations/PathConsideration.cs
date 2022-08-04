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

    public override float Calculate(Analysis analysis, Target target = null)
    {
        if(target is SpatialTarget spatialTarget)
        {
            if (spatialTarget.fleet.Busy())
            {
                return 0.0f;
            }

            if (spatialTarget.star != goal || spatialTarget.fleet.star != start)
            {
                start = spatialTarget.fleet.star;
                goal = spatialTarget.star;
                path = Master.instance.PathFind(spatialTarget.fleet.star, spatialTarget.star);
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
