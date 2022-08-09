using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositeTask", menuName = "AI/HTN/Composite Task")]
public class CompositeTask : Task, IMethod
{
    [HideInInspector] public GameState gameState;
    [HideInInspector] public int planIndex = 0;
    [HideInInspector] public int methodIndex = 0;
    public IMethod[] methods = new IMethod[1];

    [Header("FOR USE AS METHOD:")]
    public List<Consideration> considerations = new List<Consideration>();
    public float weight = 1.0f;
    public float Priority { get; private set; }

    public bool Sorted { get; private set; }

    public void CalculatePriority(Analysis analysis)
    {
        Priority = weight;
        foreach (Consideration consideration in considerations)
        {
            Priority *= consideration.Calculate(analysis);
        }
    }

    public int Compare(IMethod x, IMethod y)
    {
        return y.Priority.CompareTo(x.Priority);
    }

    public List<Task> GetTasks()
    {
        return new List<Task>() { this };
    }

    public void Reset()
    {
        Sorted = false;
        methodIndex = 0;
    }

    public void SortMethods(Analysis analysis)
    {
        for (int i = 0; i < methods.Length; ++i)
        {
            methods[i].CalculatePriority(analysis);
        }

        Array.Sort(methods);
        Sorted = true;
    }
}
