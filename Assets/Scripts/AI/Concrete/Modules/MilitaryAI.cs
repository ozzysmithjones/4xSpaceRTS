using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryAI : AIModule
{
    public Task rootTask;
    private int taskIndex = 0;
    private readonly List<Task> plan = new List<Task>();
    private GameState gameState;

    protected override void Analyse(Analysis analysis)
    {
        gameState.Clear();
        //gameState.Set(Proposition.A, true); //set propositions here.
    }

    protected override void Behave(Analysis analysis)
    {
        if(plan.Count <= 0 || taskIndex >= plan.Count)
        {
            taskIndex = 0;
            HTN.CreatePlan(rootTask, analysis, gameState, plan);
        }

        if (plan.Count > 0)
        {
            TacticTask tactic = plan[taskIndex] as TacticTask;
            TaskState state = tactic.Run(analysis, empire.military.GetFleets(FleetType.Military));

            switch (state)
            {
                case TaskState.Success:
                    ++taskIndex;
                    break;
                case TaskState.Failure:
                    plan.Clear();
                    taskIndex = 0;
                    break;
                case TaskState.Running:
                    break;
            }
        }
    }

    protected override AIModule CreateCopy()
    {
        MilitaryAI copy = ScriptableObject.CreateInstance<MilitaryAI>();
        copy.rootTask = this.rootTask;
        return copy;
    }

    protected override void OnInit()
    {

    }
}
