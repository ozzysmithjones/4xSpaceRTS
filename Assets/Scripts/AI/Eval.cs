using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Eval
{
    private static double Triangular(double number)
    {
        return number * (number + 1) / 2;
    }

    public static double EvaluateFleet(Fleet fleet)
    {
        double totalHitPoints = 0.0f;
        double totalDamage = 0.0f;

        foreach (SpaceShip spaceShip in fleet.spaceShips)
        {
            totalHitPoints += spaceShip.hitPoints;
            totalDamage += spaceShip.damage;
        }

        return totalHitPoints * totalDamage * Triangular(fleet.spaceShips.Count);
    }


    public static void PropagateFleetEvaluations(Empire empire, Analysis analysis)
    {
        InfluenceMap allianceMap = analysis.influenceMaps.GetInfluenceMap(0);
        allianceMap.Clear();

        //propagate fleet strength 

        {
            List<Fleet> fleets = empire.military.GetFleets(FleetType.Military);

            foreach (Fleet fleet in fleets)
            {
                double strength = EvaluateFleet(fleet) * 1.0f;
                allianceMap.PropagateByStar(fleet.star, 3, (star) => strength / (star.node.g + 1));
            }
        }


        InfluenceMap enemyMap = analysis.influenceMaps.GetInfluenceMap(1);
        enemyMap.Clear();

        //Propagate enemy strength across the environment.

        List<Empire> enemies = empire.military.GetEnemies();

        foreach (Empire enemy in enemies)
        {
            List<Fleet> fleets = enemy.military.GetFleets(FleetType.Military);

            foreach (Fleet fleet in fleets)
            {
                double eval = EvaluateFleet(fleet);
                enemyMap.PropagateByStar(fleet.star, 3, (star) => eval / (star.node.g + 1));
            }
        }


        InfluenceMap conflictMap = analysis.influenceMaps.GetInfluenceMap(2);

        for (int y = 0; y < conflictMap.height; ++y)
        {
            for (int x = 0; x < conflictMap.width; ++x)
            {
                double a = allianceMap[x, y];
                double b = enemyMap[x, y];

                if (a != 0 && b != 0)
                {
                    conflictMap[x, y] = a - b;
                }
                else
                {
                    conflictMap[x, y] = 0.0f;
                }
            }
        }
    }


    private static double EvaluateStar(Star star, Analysis analysis)
    {
        float value = EvaluateResources(analysis, star.starEconomy.totalResourceProduction);

        List<PlanetColony> colonies = star.starEconomy.colonies;

        foreach (PlanetColony colony in colonies)
        {
            value += EvaluateResources(analysis, colony.GetModResources());
        }

        return value;
    }

    private static float EvaluateResources(Analysis analysis, Resources resources)
    {
        float value = 0.0f;

        for (int i = 0; i < resources.amounts.Length; ++i)
        {

            switch ((ResourceType)i)
            {
                case ResourceType.FOOD:
                    value += resources.amounts[i] * analysis[ValueType.Food];
                    break;
                case ResourceType.STABILITY:
                    value += resources.amounts[i] * analysis[ValueType.Stability];
                    break;
                case ResourceType.MATERIALS:
                    value += resources.amounts[i] * analysis[ValueType.Materials];
                    break;
                case ResourceType.SCIENCE:
                    value += resources.amounts[i] * analysis[ValueType.Science] ;
                    break;
            }
        }

        return value * analysis[ValueType.Time] * (1.0f - analysis[ValueType.Threat]);
    }

    public static void PropagateStarEvaluations(Empire empire, Analysis analysis)
    {
        InfluenceMap territoryMap = analysis.influenceMaps.GetInfluenceMap(3);

        foreach (Star star in empire.territory.stars)
        {
            double eval = EvaluateStar(star, analysis);
            territoryMap.PropagateByStar(star, 1, (s) => eval / (s.node.g + 1));
        }
    }

    public static void PropagateGoalEvaluations(Empire empire, Analysis analysis)
    {
        //put code here if needed.
    }
}
