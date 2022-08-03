using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CompositeTask", menuName = "AI/HTN/Composite Task")]
public class CompositeTask : Task
{
    [HideInInspector] public GameState gameState;
    [HideInInspector] public int planIndex = 0;
    [HideInInspector] public int methodIndex = 0;
    public Method[] methods = new Method[1];

    public void SortMethods(Analysis analysis)
    {
        for (int i = 0; i < methods.Length; ++i)
        {
            methods[i].CalculatePriority(analysis);
        }

        Array.Sort(methods);
    }
}
