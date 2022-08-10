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
        InfluenceMap allyMap = analysis.allyMilitaryMap;
        allyMap.Clear();

        //propagate fleet strength 

        {
            List<Fleet> fleets = empire.military.GetFleets(FleetType.Military);

            foreach (Fleet fleet in fleets)
            {
                double strength = EvaluateFleet(fleet) * 1.0f;
                allyMap.PropagateByStar(fleet.star, 1, (star) => strength / (star.node.g * 2 + 1));
            }
        }


        InfluenceMap enemyMap = analysis.enemyMilitaryMap;
        enemyMap.Clear();

        //Propagate enemy strength across the environment.

        List<Empire> enemies = empire.military.GetEnemies();

        foreach (Empire enemy in enemies)
        {
            List<Fleet> fleets = enemy.military.GetFleets(FleetType.Military);

            foreach (Fleet fleet in fleets)
            {
                double eval = EvaluateFleet(fleet);
                enemyMap.PropagateByStar(fleet.star, 1, (star) => eval / (star.node.g * 2 + 1));
            }
        }


        InfluenceMap conflictMap = analysis.conflictMap;

        for (int i = 0; i < conflictMap.Length; ++i)
        {
            double a = allyMap[i];
            double b = enemyMap[i];

            if (a != 0 && b != 0)
            {
                conflictMap[i] = a - b;
            }
            else
            {
                conflictMap[i] = 0.0f;
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
        InfluenceMap allianceMap = analysis.allyEconomyMap;

        foreach (Star star in empire.territory.stars)
        {
            double eval = EvaluateStar(star, analysis);
            allianceMap.PropagateByStar(star, 0, (s) => eval / (s.node.g + 1));
        }

        InfluenceMap enemyMap = analysis.enemyEconomyMap;
        List<Empire> enemies = empire.military.GetEnemies();

        foreach (Empire enemy in enemies)
        {
            foreach (Star star in enemy.territory.stars)
            {
                double eval = EvaluateStar(star, analysis);
                enemyMap.PropagateByStar(star, 0, (s) => eval / (s.node.g + 1));
            }
        }
    }


    public static void EvaluateThreat(Empire empire, Analysis analysis, int threatDistance = 4)
    {
        List<Empire> enemies = empire.military.GetEnemies();
        int minDistance = int.MaxValue;

        foreach(Empire enemy in enemies)
        {
            List<Fleet> enemyFleets = enemy.military.GetFleets(FleetType.Military);

            foreach (Fleet fleet in enemyFleets)
            {
                if (fleet.star == null)
                    continue;

                List<Star> pathToEmpire = Master.instance.PathFind(fleet.star, (s) => s.empire == empire);
                minDistance = Mathf.Min(pathToEmpire.Count, minDistance);
            }
        }

        if (minDistance >= threatDistance)
        {
            analysis[ValueType.Threat] = 0.0f;
        }
        else
        {
            analysis[ValueType.Threat] = 1.0f - ((float)minDistance / threatDistance);
        }
    }

}
