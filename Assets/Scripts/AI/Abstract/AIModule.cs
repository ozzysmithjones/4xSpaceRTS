using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueType
{
    Threat,
}


[System.Serializable]
public class Analysis
{
    private readonly float[] valueByType = new float[Enum.GetValues(typeof(ValueType)).Length];

    public float this[ValueType type]
    {
        get => valueByType[(int)type];
        set
        {
            valueByType[(int)type] = value;
        }
    }

    public void Copy(Analysis other)
    {
        Array.Copy(other.valueByType, valueByType, valueByType.Length);
    }
}

public abstract class AIModule : ScriptableObject
{
    protected Empire empire;
    [SerializeField] private List<AIModule> subModules = new List<AIModule>();
    private readonly Analysis analysis = new Analysis();

    public void Init(Empire empire)
    {
        this.empire = empire;
        OnInit();

        for (int i = 0; i < subModules.Count; ++i)
        {
            subModules[i].Init(empire);
        }
    }

    public AIModule Clone()
    {
        AIModule clone = CreateCopy();
        clone.subModules.Clear();

        for(int i = 0; i < subModules.Count; ++i)
        {
            clone.subModules.Add(subModules[i].Clone());
        }

        return clone;
    }

    public void Behave(Analysis analysis)
    {
        this.analysis.Copy(analysis);
        OnAnalyse(this.analysis);
        OnBehave(this.analysis);

        foreach(AIModule module in subModules)
        {
            module.Behave(this.analysis);
        }
    }

    public void Add(AIModule module)
    {
        subModules.Add(module);
    }

    public void Remove(AIModule module)
    {
        subModules.Remove(module);
    }

    protected abstract AIModule CreateCopy();
    protected abstract void OnInit();
    protected abstract void OnAnalyse(Analysis analysis);
    protected abstract void OnBehave(Analysis analysis);
}


