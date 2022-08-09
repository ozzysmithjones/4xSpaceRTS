using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Method", menuName = "AI/HTN/Method")]
public class Method : ScriptableObject, IComparer<Method>
{
    public List<Task> tasks = new List<Task>();
    public List<Consideration> considerations = new List<Consideration>();
    public float weight = 1.0f;

    public float Priority { get; private set; }

    public void CalculatePriority(Analysis analysis)
    {
        Priority = weight;
        foreach (Consideration consideration in considerations)
        {
            Priority *= consideration.Calculate(analysis);
        }
    }

    public int Compare(Method x, Method y)
    {
        return y.Priority.CompareTo(x.Priority);
    }

    public List<Task> GetTasks()
    {
        return tasks;
    }
}
