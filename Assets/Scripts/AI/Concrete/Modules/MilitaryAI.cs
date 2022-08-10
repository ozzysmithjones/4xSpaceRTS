using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryAI : AIModule
{
    public Task rootTask;
    private int taskIndex = 0;
    private readonly List<Task> plan = new List<Task>();
    private readonly GameState gameState = new GameState();

    protected override void Analyse(Analysis analysis)
    {
        gameState.Clear();
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
            TaskState state = tactic.Run(empire, analysis, empire.military.GetFleets(FleetType.Military));

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
        copy.taskIndex = 0;
        return copy;
    }

    protected override void OnInit()
    {

    }
}
