using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueType
{
    Threat,
    SaveMaterials,
}


[System.Serializable]
public class Analysis
{
    public HierarchicalInfluenceMap influenceMaps;
    private readonly float[] valueByType = new float[Enum.GetValues(typeof(ValueType)).Length];

    public float this[ValueType type]
    {
        get => valueByType[(int)type];
        set
        {
            valueByType[(int)type] = value;
        }
    }

    public void Clear()
    {
        influenceMaps.Clear();

        for(int i = 0; i < valueByType.Length; ++i)
        {
            valueByType[i] = 0.0f;
        }
    }
}

public abstract class AIModule : ScriptableObject
{
    protected Empire empire;
    [SerializeField] private List<AIModule> subModules = new List<AIModule>();

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

    public void Update(Analysis analysis)
    {
        Analyse(analysis);
        Behave(analysis);

        foreach(AIModule module in subModules)
        {
            module.Update(analysis);
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
    protected abstract void Analyse(Analysis analysis);
    protected abstract void Behave(Analysis analysis);
}


