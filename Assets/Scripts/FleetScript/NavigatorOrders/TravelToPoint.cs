using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPoint : FleetOrder
{
    protected List<int> path;
    protected Transform point;
    public TravelToPoint(Fleet fleet,int starCoordinate,Transform point = null, List<int> initialPath = null) : base(fleet)
    {
        if(initialPath == null)
        {
            this.path = Master.instance.PathFind(fleet.star.index, starCoordinate);
        }
        else
        {
            path = new List<int>(initialPath);
        }

        if(point == null)
        {
            this.point = Master.instance.enviroment.stars[starCoordinate].transform;
        }
        else
        {
            this.point = point;
        }
    }

    public override void Initialise(Fleet fleet)
    {
        base.Initialise(fleet);

        if(fleet.star.index == path[0])
        {
            fleet.SetPath(path);
        }
        else if(fleet.star.index != path[path.Count-1])
        {
            this.path = Master.instance.PathFind(fleet.star.index, path[path.Count - 1]);
            fleet.SetPath(path);
        }
        if (fleet.star.index == path[path.Count - 1])
        {
            fleet.SetTarget(point);
        }


    }

    public override bool Completed()
    {
        if(fleet.star.index == path[path.Count-1] && fleet.isCloseToTarget && fleet.isTarget && fleet.target == point)
        {
            OnCompleted();
            return true;
        }
        else
        {
            return false;
        }
       
    }
    public override void GetTask()
    {
        base.GetTask();
        if(fleet.star.index == path[path.Count - 1] && !fleet.isTarget)
        {
            fleet.SetTarget(point);
            return;
        }
        if (!fleet.isPath)
        {
            this.path = Master.instance.PathFind(fleet.star.index, path[path.Count-1]);
            fleet.SetPath(path);
            return;
        }
       
    
    }

    protected virtual void OnCompleted()
    {

    }


}
