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
    public abstract TaskState Run(Empire empire, Analysis analysis, List<Fleet> fleets);

    private static double PathEval(Fleet fleet, double fleetPower, Analysis analysis, Star star)
    {
        InfluenceMap alliance = analysis.influenceMaps[0];
        InfluenceMap enemy = analysis.influenceMaps[1];
        InfluenceMap allianceEconomy = analysis.influenceMaps[3];
        InfluenceMap enemyEconomy = analysis.influenceMaps[4];

        double score = 0;
        double enemyPower = enemy[star.x, star.y];

        if (enemyPower > 0.0f)
        {
            score = fleetPower - enemyPower;

            if (score < 0.0f) //do not engage if lose the battle.
            {
                return score;
            }
        }

        score += (analysis[ValueType.Reinforce] - analysis[ValueType.Disperse]) * alliance[star.x, star.y];
        score += analysis[ValueType.DefendTerritory] * allianceEconomy[star.x, star.y];
        score += analysis[ValueType.InvadeTerritory] * enemyEconomy[star.x, star.y];
        score += star.empire == fleet.empire ? -10 : 10;
        score += (ChokePointDetection.GetThroughput(star) >> 2);

        return score;
    }


    protected List<Star> EscapePath(Analysis analysis,Fleet fleet, double fleetPower)
    {
        InfluenceMap enemyMap = analysis.influenceMaps[1];
        List<Star> stars = Master.instance.Presence(fleet.star, 1);

        foreach(Star star in stars)
        {
            double enemyPower = enemyMap[star.x, star.y];

            if(enemyPower > fleetPower)
            {
                return Master.instance.PathFind(fleet.star, 2, (s) => PathEval(fleet, fleetPower, analysis, s));
            }
        }

        return null;
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
