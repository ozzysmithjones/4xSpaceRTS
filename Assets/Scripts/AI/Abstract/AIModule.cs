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
    private readonly Dictionary<ValueType, float> valueByType = new Dictionary<ValueType, float>();

    public float this[ValueType type]
    {
        get => valueByType.TryGetValue(type, out float value) ? value : 0.0f;
        set
        {
            if(valueByType.ContainsKey(type))
            {
                valueByType[type] = value;

            }else
            {
                valueByType.Add(type, value);
            }
        }
    }

    public void Copy(Analysis other)
    {
        valueByType.Clear();

        foreach(var pair in other.valueByType)
        {
            valueByType.Add(pair.Key, pair.Value);
        }
    }
}

public abstract class AIModule : MonoBehaviour
{
    [SerializeField] private List<AIModule> subModules = new List<AIModule>();
    private Analysis analysis = new Analysis();

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

    protected abstract void OnAnalyse(Analysis analysis);
    protected abstract void OnBehave(Analysis analysis);
}


