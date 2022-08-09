using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMethod : IComparer<IMethod>
{
    List<Task> GetTasks();

    float Priority { get; }

    void CalculatePriority(Analysis analysis);
}

[System.Serializable]
[CreateAssetMenu(fileName = "Method", menuName = "AI/HTN/Method")]
public class Method : ScriptableObject, IMethod
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

    public int Compare(IMethod x, IMethod y)
    {
        return y.Priority.CompareTo(x.Priority);
    }

    public List<Task> GetTasks()
    {
        return tasks;
    }
}
