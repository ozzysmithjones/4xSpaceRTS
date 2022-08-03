using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum Proposition
{
    TEST,
}

public class GameState
{
    readonly bool[] propositions = new bool[System.Enum.GetValues(typeof(Proposition)).Length];

    public bool Has(Proposition proposition)
    {
        return propositions[(int)proposition];
    }

    public void Set(Proposition proposition, bool value)
    {
        propositions[(int)proposition] = value;
    }

    public void Clear()
    {
        for(int i = 0; i < propositions.Length; ++i)
        {
            propositions[i] = false;
        }
    }

    public void CopyTo(GameState gameState)
    {
        System.Array.Copy(propositions, gameState.propositions, gameState.propositions.Length);
    }

    public GameState Clone()
    {
        GameState gameState = new GameState();
        System.Array.Copy(propositions, gameState.propositions, gameState.propositions.Length);
        return gameState;
    }
}

[System.Serializable]
public struct Condition
{
    public Proposition proposition;
    public bool value;

    public Condition(Proposition proposition, bool value)
    {
        this.proposition = proposition;
        this.value = value;
    }
}

public abstract class Task : ScriptableObject
{
    [SerializeField] private List<Condition> prerequisites = new List<Condition>();
    //[SerializeField] protected List<Effect> effects = new List<Effect>();

    public bool Applicable(GameState gameState)
    {
        for(int i = 0; i < prerequisites.Count; ++i)
        {
            if(gameState.Has(prerequisites[i].proposition) != prerequisites[i].value)
            {
                return false;
            }
        }

        return true;
    }
}




public class HTN
{
    public Task rootTask;

    private void Init(Analysis analysis)
    {
        if(!(rootTask is CompositeTask))
        {
            return;
        }
        
        Queue<CompositeTask> open = new Queue<CompositeTask>();
        open.Enqueue((CompositeTask)rootTask);

        while (open.Count > 0)
        {
            CompositeTask compositeTask = open.Dequeue();
            compositeTask.SortMethods(analysis);

            for (int i = 0; i < compositeTask.methods.Length; ++i)
            {
                List<Task> tasks = compositeTask.methods[i].tasks;

                for (int j = 0; j < tasks.Count; ++j)
                {
                    if (tasks[j] is CompositeTask child)
                    {
                        open.Enqueue(child);
                    }
                }
            }
        }
    }

    public void CreatePlan(Analysis analysis,GameState gameState,List<Task> plan)
    {
        Init(analysis);

        plan.Clear();
        int planIndex = 0;

        plan.Add(rootTask);
        Stack<CompositeTask> history = new Stack<CompositeTask>();

        while (planIndex < plan.Count)
        {
            if(!plan[planIndex].Applicable(gameState))
            {
                if(history.Count <= 0)
                {
                    plan.Clear();
                    return;
                }

                //backtracking.

                CompositeTask current = null;
                while (history.Count > 0)
                {
                    current = history.Peek();
                    Method currentMethod = current.methods[current.methodIndex];
                    plan.RemoveRange(current.planIndex, currentMethod.tasks.Count);
                    ++current.methodIndex;

                    planIndex = current.planIndex;
                    plan.Add(current);

                    if(current.methodIndex < current.methods.Length)
                    {
                        break;
                    }

                    history.Pop();
                };

                current.gameState.CopyTo(gameState);
            }

            if (plan[planIndex] is PrimitiveTask primitiveTask)
            {
                primitiveTask.Apply(gameState);
                ++planIndex;
            }
            else if (plan[planIndex] is CompositeTask compositeTask)
            {
                compositeTask.planIndex = planIndex;
                gameState.CopyTo(compositeTask.gameState);

                plan.RemoveAt(planIndex);
                plan.InsertRange(planIndex, compositeTask.methods[compositeTask.methodIndex].tasks);

                history.Push(compositeTask);
            }
        }
    }
}
