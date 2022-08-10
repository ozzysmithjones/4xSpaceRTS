using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fleet AI", menuName = "AI/Modules/Fleet AI")]
public class FleetAI : AIModule
{

    protected override void Analyse(Analysis analysis)
    {
        analysis[ValueType.Reinforce] = 0.0f;
        analysis[ValueType.Disperse] = 0.0f;
        analysis[ValueType.DefendTerritory] = 0.0f;
        analysis[ValueType.InvadeTerritory] = 2.0f;

        analysis[ValueType.Food] = 10.0f;
        analysis[ValueType.Materials] = 10.0f;
        analysis[ValueType.Stability] = 10.0f;
        analysis[ValueType.Science] = 10.0f;
        analysis[ValueType.Time] = 100.0f;

        Eval.PropagateFleetEvaluations(empire, analysis);
        Eval.PropagateStarEvaluations(empire, analysis);
    }

    private static double Evaluate(Fleet fleet, double fleetPower, Analysis analysis,Star star)
    {
        InfluenceMap alliance = analysis.allyMilitaryMap;
        InfluenceMap enemy = analysis.enemyMilitaryMap;
        InfluenceMap allianceEconomy = analysis.allyEconomyMap;
        InfluenceMap enemyEconomy = analysis.enemyEconomyMap;

        double score = 0;
        double enemyPower = enemy[star.index];

        if (enemyPower > 0.0f) 
        {
            score = fleetPower - enemyPower;

            if (score < 0.0f) //do not engage if lose the battle.
            {
                return score;
            }
        }

        score += (analysis[ValueType.Reinforce] - analysis[ValueType.Disperse]) * alliance[star.index];
        score += analysis[ValueType.DefendTerritory] * allianceEconomy[star.index];
        score += analysis[ValueType.InvadeTerritory] * enemyEconomy[star.index];
        score += star.empire == fleet.empire ? -10 : 10;

        return score;
    }

    protected override void Behave(Analysis analysis)
    {
        List<Fleet> fleets = empire.military.GetFleets(FleetType.Military);

        foreach(Fleet fleet in fleets)
        {
            if(!fleet.Busy() && (!fleet.HasOrders() || Time.time > fleet.TimeSinceLastOrder + 20.0f))
            {
                fleet.ClearOrders();
                double fleetPower = Eval.EvaluateFleet(fleet);
                List<Star> path = Master.instance.PathFind(fleet.star, 3, (s) => Evaluate(fleet, fleetPower, analysis, s));

                if (path.Count >= 2)
                {
                    fleet.AddOrder(new MoveOrder(path[path.Count - 1], path[path.Count - 1].transform, path));
                }
            }
        }
    }

    protected override AIModule CreateCopy()
    {
        return ScriptableObject.CreateInstance<FleetAI>();
    }

    protected override void OnInit()
    {

    }
}
