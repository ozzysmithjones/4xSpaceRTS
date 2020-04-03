using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetOrder
{
    public bool initialised = false;
    protected Fleet fleet;
    protected bool interrupted = false;

    public FleetOrder(Fleet fleet)
    {
        this.fleet = fleet;
    }
    public virtual void Initialise(Fleet fleet)
    {
        fleet.ClearPath();
        fleet.ClearTarget(true);
        initialised = true;
    }

    public virtual bool Completed()
    {
        return true;
    }

    public virtual void GetTask()
    {
        interrupted = false;
    }
    public virtual void SetInterrupt(bool interrupted)
    {
        this.interrupted = interrupted;

    }

}
