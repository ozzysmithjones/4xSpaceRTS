using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ValueType
{
    Threat,      //value inddicating prioritisation of military over production.

    //Prioritisation of resources:

    Food,
    Stability,
    Materials,
    Science,

    Time,        //Value indicating how much time before next war (useful for production and military planning). 
    Reinforce,   //Value indication priority to reinforce other fleets.
    Disperse,    //Value indication priority to seperate from other fleets.
    DefendTerritory,     //Value indicating priority for defending territory.
    InvadeTerritory,     //Value indicating priority for invading terruo
}

public class Analysis
{
  //  public HierarchicalInfluenceMap influenceMaps = new HierarchicalInfluenceMap(5, 45);
    private readonly float[] valueByType = new float[Enum.GetValues(typeof(ValueType)).Length];

    public readonly InfluenceMap allyMilitaryMap;
    public readonly InfluenceMap allyEconomyMap;
    public readonly InfluenceMap enemyMilitaryMap;
    public readonly InfluenceMap enemyEconomyMap;
    public readonly InfluenceMap conflictMap;
    public readonly InfluenceMap throughputMap;

    public Analysis(int numStars)
    {
        this.allyMilitaryMap = new InfluenceMap(numStars);
        this.enemyMilitaryMap = new InfluenceMap(numStars);
        this.allyEconomyMap = new InfluenceMap(numStars);
        this.enemyEconomyMap = new InfluenceMap(numStars);
        this.throughputMap = new InfluenceMap(numStars);
    }

    public float this[ValueType type]
    {
        get => valueByType[(int)type];
        set
        {
            valueByType[(int)type] = value;
        }
    }

    public void ClearValues()
    {
        for(int i = 0; i < valueByType.Length; ++i)
        {
            valueByType[i] = 0.0f;
        }
    }

    public void ClearInfluenceMaps()
    {
        allyMilitaryMap.Clear();
        allyEconomyMap.Clear();
        enemyMilitaryMap.Clear();
        enemyEconomyMap.Clear();
        conflictMap.Clear();
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

    public void UpdateAI(Analysis analysis)
    {
        Analyse(analysis);
        Behave(analysis);

        foreach(AIModule module in subModules)
        {
            module.UpdateAI(analysis);
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


