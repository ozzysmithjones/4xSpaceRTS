using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleetOrder
{
    public bool initialised = false;
    protected Fleet fleet;
    public virtual void Initialise(Fleet fleet)
    {
        initialised = true;
        this.fleet = fleet;
    }

    public virtual bool Execute()
    {
        return true;
    }
}
