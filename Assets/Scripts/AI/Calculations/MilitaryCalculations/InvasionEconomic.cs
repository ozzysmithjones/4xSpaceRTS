using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Calculation
{
    /*
     * given an array of enemies, returns the improved expansion rate by building a military ship.(this expansion rate can be compared to just expanding).
     * This calculation only works if the cost of building the weakest ship and expanding are the same.
     */
    public static float InvasionEconomic(Faction ai,Faction enemy)
    {
        if(ai.colonies.Count <= 0)
        {
            Debug.Log("this AI has no colonies");
            return 0.0f;
        }

        float InvasionWealth = enemy.resourceProduction.Total();
        float time = TimeToMoveThroughStar() * enemy.territory.Count + (TimeToreachFaction(ai.colonies[0].index, enemy.factionIndex));

        float allyFleetStr = TotalFleetStrength(ai.fleets);
        float enemyFleetStr = TotalFleetStrength(enemy.fleets);

        if(allyFleetStr + enemyFleetStr <= 0)
        {
            return (InvasionWealth / time);
        }


        float victoryChance = allyFleetStr / (allyFleetStr + enemyFleetStr);
        float militaryShipImprovement = (allyFleetStr + 1) / (allyFleetStr + enemyFleetStr);
        float diff = militaryShipImprovement - victoryChance;

        //  Debug.Log("Invasion economic = " + InvasionWealth / time);
        return (InvasionWealth / time) * diff;
    }
}
