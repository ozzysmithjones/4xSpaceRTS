using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskState
{
    Success,
    Failure,
    Running,
}

public abstract class TacticTask : PrimitiveTask
{
    public abstract void Begin(Empire empire, Analysis analysis);
    public abstract TaskState Run(Empire empire, Analysis analysis, List<Fleet> fleets);
    public abstract void End(Empire empire, Analysis analysis);


    private static double PathEval(Fleet fleet, double fleetPower, Analysis analysis, Star star)
    {
        InfluenceMap allyMilitary = analysis.allyMilitaryMap;
        InfluenceMap enemyMilitary = analysis.enemyMilitaryMap;
        InfluenceMap allyEconomy = analysis.allyEconomyMap;
        InfluenceMap enemyEconomy = analysis.enemyEconomyMap;

        double score = 0;
        double enemyPower = enemyMilitary[star.index];

        if (enemyPower > 0.0f)
        {
            score = fleetPower - enemyPower;

            if (score < 0.0f) //do not engage if lose the battle.
            {
                return score;
            }
        }

        score += (analysis[ValueType.Reinforce] - analysis[ValueType.Disperse]) * allyMilitary[star.index];
        score += analysis[ValueType.DefendTerritory] * allyEconomy[star.index];
        score += analysis[ValueType.InvadeTerritory] * enemyEconomy[star.index];
        score += star.empire == fleet.empire ? -10 : 10;
        score += ChokePointDetection.GetCongestion(star);

        return score;
    }


    protected bool AssignEscapePath(Analysis analysis,Fleet fleet, double fleetPower)
    {
        InfluenceMap enemyMap = analysis.enemyMilitaryMap;
        List<Star> stars = Master.instance.Presence(fleet.star, 1);

        foreach(Star star in stars)
        {
            double enemyPower = enemyMap[star.index];

            if(enemyPower > fleetPower)
            {
                List<Star> path = Master.instance.PathFind(fleet.star, 2, (s) => PathEval(fleet, fleetPower, analysis, s));
                fleet.AddOrder(new MoveOrder(path[path.Count - 1], path[path.Count - 1].transform, path));
                return true;
            }
        }

        return false;
    }

    protected List<Star> AntPath(Fleet fleet, double fleetPower, Analysis analysis, int depth)
    {
        return Master.instance.PathFind(fleet.star, depth, (s) => PathEval(fleet, fleetPower, analysis, s));
    }


    protected List<Star> RandomPath(Fleet fleet)
    {
        return Master.instance.PathFind(fleet.star, 3, (s) => Random.Range(1.0f, 100.0f));
    }
}
