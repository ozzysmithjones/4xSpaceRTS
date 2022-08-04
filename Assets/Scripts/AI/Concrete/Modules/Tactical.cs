using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tactical
{

    private static double Triangular(double number)
    {
        return number * (number + 1) / 2;
    }

    public static double CalculateStrength(Fleet fleet)
    {
        double value = 0.0f;

        foreach(SpaceShip spaceShip in fleet.spaceShips)
        {
            value += spaceShip.damage * spaceShip.hitPoints;
        }

        return value * Triangular(fleet.spaceShips.Count);
    }


    public static void AnalyseFleetStrengths(Empire empire, Analysis analysis)
    {
        InfluenceMap militaryMap = analysis.influenceMaps.GetInfluenceMap(0);
        militaryMap.Clear();

        //propagate fleet strength 

        {
            List<Fleet> fleets = empire.military.GetFleets(FleetType.Military);

            foreach(Fleet fleet in fleets)
            {
                double strength = CalculateStrength(fleet) * 1.0f;
                militaryMap.PropagateByStar(fleet.star, 3, (star) => strength / (star.node.g + 1));
            }
        }

        //Propagate enemy strength across the environment.

        List<Empire> enemies = empire.military.GetEnemies();

        foreach(Empire enemy in enemies)
        {
            List<Fleet> fleets = enemy.military.GetFleets(FleetType.Military);

            foreach(Fleet fleet in fleets)
            {
                double strength = CalculateStrength(fleet) * -1.0f;
                militaryMap.PropagateByStar(fleet.star, 3, (star) => strength / (star.node.g + 1) );
            }
        }
    }
}
