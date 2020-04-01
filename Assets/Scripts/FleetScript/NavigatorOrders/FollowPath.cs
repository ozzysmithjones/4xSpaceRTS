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
        fleet.SetPath(path);
        base.Initialise(fleet);
    }

    public override bool Execute()
    {
        return (fleet.star.index == path[path.Count - 1]);
    }
}
