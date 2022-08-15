using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Target :IComparer<Target>
{
    public int utility;

    public int Compare(Target x, Target y)
    {
        return y.utility.CompareTo(x.utility);
    }
}

[System.Serializable]
public class SpatialTarget : Target, IComparer<SpatialTarget>
{
    public Star star;
    public Planet planet;
    public Fleet fleet;

    public int Compare(SpatialTarget x, SpatialTarget y)
    {
        return y.utility.CompareTo(x.utility);
    }
}

[System.Serializable]
public class BuildTarget : Target, IComparer<BuildTarget>
{
    public BuildQueueItem item;

    public int Compare(BuildTarget x, BuildTarget y)
    {
        return y.utility.CompareTo(x.utility);
    }
}
