using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TechType
{
    MILITARY,
    ECONOMY,
    DIPLOMACY,
    INTERNAL_POLITICS
}

public class ResearchQueueItem : ScriptableObject
{

    private ResearchQueueItem previousResearch;
    public int cost = 100;
    public bool repeatable = false;
    public ResearchQueueItem[] nextResearch = new ResearchQueueItem[0];

    public Sprite sprite;
    public new string name;
    public string description;
    public TechType techType;

    public virtual void FinishResearch(Faction faction)
    {
       
    }
    public void SetPreviousResearch(ResearchQueueItem researchQueueItem)
    {
        this.previousResearch = researchQueueItem;
    }
    public ResearchQueueItem GetPreviousResearch()
    {
        return previousResearch;
    }
    public virtual ResearchQueueItem[] GetNextResearch()
    {
        for(int i = 0; i < nextResearch.Length; i++)
        {
            nextResearch[i].SetPreviousResearch(this);
        }
        return nextResearch;
    }
}
