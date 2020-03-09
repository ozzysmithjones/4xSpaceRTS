using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Calculation
{
    public static float FleetStrength(List<SpaceShip> fleet)
    {
        return fleet.Count;
    }

    public static float TotalFleetStrength(List<Navigator> navigators)
    {
        float strength = 0.0f;
        for(int i = 0; i < navigators.Count; i++)
        {
            strength += FleetStrength(navigators[i].spaceShips);
        }
        return strength;
    }
}
