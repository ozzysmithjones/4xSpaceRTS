using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : FleetOrder
{
    private List<int> path;

    public FollowPath(List<int> path)
    {
        this.path = new List<int>(path);
    }

    public override void Initialise(Fleet fleet)
    {
        if (fleet.star.index == path[0])
        {
            fleet.SetPath(path);
        }
        else
        {
            path = GraphSearchAlgorithms.instance.PathFind(fleet.star.index, path[path.Count - 1]);
            fleet.SetPath(path);
        }
        base.Initialise(fleet);
    }

    public override bool Execute()
    {
        if (!fleet.isPath && fleet.star.index != path[path.Count - 1])
        {
            path = GraphSearchAlgorithms.instance.PathFind(fleet.star.index, path[path.Count - 1]);
            fleet.SetPath(path);
        }
        return (fleet.star.index == path[path.Count - 1]);
    }
}
