using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToStar : FleetOrder
{
    private int star = -1;
    private bool usingGates = true;
    public TravelToStar(int star, bool usingGates)
    {
        this.star = star;
        this.usingGates = usingGates;
    }
    public override void Initialise(Fleet fleet)
    {
        base.Initialise(fleet);
        fleet.SetPath(GraphSearchAlgorithms.instance.PathFind(fleet.star.index, star), usingGates);
    }

    public override bool Execute()
    {
        if(fleet.star.index == star)
        {
            return true;
        }

        if (!fleet.isPath)
        {
            fleet.SetPath(GraphSearchAlgorithms.instance.PathFind(fleet.star.index, star), usingGates);
        }

        return false;
    }

}
