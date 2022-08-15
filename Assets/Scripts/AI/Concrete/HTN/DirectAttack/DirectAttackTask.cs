using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DirectAttackTask", menuName = "AI/HTN/Direct Attack")]
public class DirectAttackTask : TacticTask
{
    public override void Begin(Empire empire, Analysis analysis)
    {
        throw new System.NotImplementedException();
    }

    public override void End(Empire empire, Analysis analysis)
    {
        throw new System.NotImplementedException();
    }

    public override TaskState Run(Empire empire, Analysis analysis, List<Fleet> fleets)
    {
        if (fleets.Count <= 0)
        {
            return TaskState.Failure;
        }

        Fleet target = null;
        double targetPower = double.MaxValue;

        //Get strongest enemy fleet.
        List<Empire> enemies = empire.military.GetEnemies();
        int numWithoutFleets = 0;

        foreach(Empire enemy in enemies)
        {
            List<Fleet> enemyFleets = enemy.military.GetFleets(FleetType.Military);

            if(enemyFleets.Count <= 0)
            {
                ++numWithoutFleets;
            }

            foreach(Fleet fleet in enemyFleets)
            {
                double power = Eval.EvaluateFleet(fleet);
                if(power < targetPower)
                {
                    targetPower = power;
                    target = fleet;
                }
            }
        }

        if(numWithoutFleets == enemies.Count)
        {
            return TaskState.Success;
        }

        foreach(Fleet fleet in fleets)
        {
            if (!fleet.Busy() && (!fleet.HasOrders() || Time.time > fleet.TimeSinceLastOrder + 20.0f))
            {
                fleet.ClearOrders();
                double fleetPower = Eval.EvaluateFleet(fleet);

                //First try an escape path if the fleet needs escaping.

                if(AssignEscapePath(analysis,fleet, fleetPower))
                {
                    continue;
                }

                List<Star> path;
                if (fleetPower >= targetPower)
                {
                    //Then try to target the weakest enemy fleet.
                    path = Master.instance.PathFind(fleet.star, target.star);
                    if (path != null && path.Count >= 2)
                    {
                        fleet.AddOrder(new MoveOrder(path[path.Count - 1], path[path.Count - 1].transform, path));
                    }
                }
                else
                {
                    //TODO: consider regrouping if the enemy fleet is stronger.
                    path = AntPath(fleet,fleetPower, analysis, 3);
                    if (path != null && path.Count >= 2)
                    {
                        fleet.AddOrder(new MoveOrder(path[path.Count - 1], path[path.Count - 1].transform, path));
                    }
                }
            }
        }

        return TaskState.Running;
    }
}
