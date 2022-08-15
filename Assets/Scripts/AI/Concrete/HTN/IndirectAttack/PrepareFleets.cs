using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "PrepareFleets", menuName = "AI/HTN/PrimitiveTask")]
public class PrepareFleets : TacticTask
{
    public Rendezvous[] rendezvouses = new Rendezvous[0];
    public double minRelativeFleetPower = 1.0;

    public override TaskState Run(Empire empire, Analysis analysis, List<Fleet> fleets)
    {
        foreach(Rendezvous target in rendezvouses)
        {
            if(target.star == null)
            {
                UpdateRendezvouses(empire, analysis);
                break;
            }
        }

        foreach(Empire enemy in empire.military.GetEnemies())
        {
            foreach(Fleet enemyFleet in enemy.military.GetFleets(FleetType.Military))
            {
                List<Star> path = Master.instance.PathFind(enemyFleet.star, (s) => s.empire == empire, 2);
                if(path != null && path.Count >= 2)
                {
                    return TaskState.Success; //enemy has invaded, no more preperation allowed.
                }
            }
        }

        int numReached = 0;
        foreach(Fleet fleet in fleets)
        {
            if (!fleet.Busy() && (!fleet.HasOrders() || Time.time > fleet.TimeSinceLastOrder + 20.0f))
            {
                fleet.ClearOrders();
                if (AssignEscapePath(analysis, fleet, fleet.Power))
                {
                    continue;
                }

                bool reached = false;
                foreach (Rendezvous target in rendezvouses)
                {
                    if (target.star != null && target.star == fleet.star)
                    {
                        reached = true;
                        break;
                    }
                }

                if (reached)
                {
                    ++numReached;
                    continue;
                }

                List<Star> path;
                Rendezvous rendezvous = rendezvouses[Random.Range(0, rendezvouses.Length)];
                if (rendezvous.star == null)
                {
                    path = AntPath(fleet, fleet.Power, analysis, 3);
                }
                else
                {
                    path = Master.instance.PathFind(fleet.star, rendezvous.star);
                }

                if (path != null && path.Count >= 2)
                {
                    fleet.AddOrder(new MoveOrder(path[path.Count - 1], path[path.Count - 1].transform, path));
                }
            }
        }


        return (float)numReached >= (fleets.Count * 0.75f) ? TaskState.Success : TaskState.Running;
    }

    public override void Begin(Empire empire, Analysis analysis)
    {
        UpdateRendezvouses(empire, analysis);
    }

    private void UpdateRendezvouses(Empire empire, Analysis analysis)
    {
        /*
        List<Star> exceptions = new List<Star>();

        foreach (Rendezvous rendezvous in rendezvouses)
        {
            rendezvous.star = null;

            switch (rendezvous.type)
            {
                case RendezvousType.Defend:
                    rendezvous.star = Eval.GetDefendStar(empire, exceptions);
                    break;
                case RendezvousType.Attack:
                    rendezvous.star = Eval.GetAttackStar(empire, analysis, exceptions);
                    break;
                case RendezvousType.Ambush:
                    rendezvous.star = Eval.GetAmbushStar(empire, analysis, exceptions);
                    break;
            }

            exceptions.Add(rendezvous.star);
        }
        */
    }

    public override void End(Empire empire, Analysis analysis)
    {

    }
}
