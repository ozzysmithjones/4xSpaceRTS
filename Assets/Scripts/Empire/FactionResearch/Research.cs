using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Research
{
    public enum ResearchEventType
    {
        Begin,
        Update,
        Finish,
        Stop
    }

    public bool researching = false;
    public ResearchQueueItem researchQueueItem;
    [System.NonSerialized] private Empire empire;

    public List<ResearchQueueItem> researchOptions = new List<ResearchQueueItem>();

    public delegate void OnResearchEvent(Research research,ResearchQueueItem researchQueueItem);
    public event OnResearchEvent ResearchBegin;
    public event OnResearchEvent ResearchUpdate;
    public event OnResearchEvent ResearchFinish;
    public event OnResearchEvent ResearchStop;

    public Research(Empire faction)
    {
        this.empire = faction;
    }
    public void Update()
    {
        if (!researching || researchQueueItem == null)
        {
            return;
        }
        if(ResearchUpdate != null) ResearchUpdate.Invoke(this,this.researchQueueItem);
        if (empire.economy.Pay(researchQueueItem.cost, ResourceType.SCIENCE))
        {
            researchQueueItem.FinishResearch(empire);
            researchOptions.AddRange(researchQueueItem.GetNextResearch());
            if (ResearchFinish != null) ResearchFinish.Invoke(this,researchQueueItem);
            StopResearch();
        }
    }
    public void ResearchAtRandom()
    {
        BeginResearch(researchOptions[Random.Range(0, researchOptions.Count)]);
    }

    public void BeginResearch(ResearchQueueItem researchQueueItem)
    {
        this.researchQueueItem = researchQueueItem;
        researching = true;
        if (ResearchBegin != null) ResearchBegin.Invoke(this,this.researchQueueItem);
        researchOptions.Remove(researchQueueItem);
    }
    public void StopResearch()
    {
        empire.economy.SetResourceAmount(ResourceType.SCIENCE, 0);
        researching = false;
        if(ResearchStop != null) ResearchStop.Invoke(this,this.researchQueueItem);
        this.researchQueueItem = null;
    }
    public float Getprogress()
    {
        if (!researching)
        {
            return 1.0f;
        }
 
        return (float)empire.economy.resources.amounts[(int)ResourceType.SCIENCE] / (float)researchQueueItem.cost;
    }

    public void EventListener(ResearchEventType researchEventType,OnResearchEvent onResearchEvent, bool listen)
    {
        switch (researchEventType)
        {
            case ResearchEventType.Begin:
                if (listen)
                {
                    ResearchBegin += onResearchEvent;
                }
                else
                {
                    ResearchBegin -= onResearchEvent;
                }
                break;
            case ResearchEventType.Update:
                if (listen)
                {
                    ResearchUpdate += onResearchEvent;
                }
                else
                {
                    ResearchUpdate -= onResearchEvent;
                }
                break;
            case ResearchEventType.Finish:
                if (listen)
                {
                    ResearchFinish += onResearchEvent;
                }
                else
                {
                    ResearchFinish -= onResearchEvent;
                }
                break;
            case ResearchEventType.Stop:
                if (listen)
                {
                    ResearchStop += onResearchEvent;
                }
                else
                {
                    ResearchStop -= onResearchEvent;
                }
                break;
            default:
                Debug.LogError("couldn't find the research event " + researchEventType.ToString());
                return;

        }
    }



}
