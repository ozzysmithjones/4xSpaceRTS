using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchOptionsOverview : MonoBehaviour
{
    public Color[] colors;
    public GameObject researchOptionPrefab;
    private List<ResearchOptionUI> researchOptionUIs = new List<ResearchOptionUI>();

    private int maxPoolSize = 10;
    private List<ResearchOptionUI> pool = new List<ResearchOptionUI>();

    private void Start()
    {
        for(int i = 0; i < maxPoolSize; i++)
        {
            pool.Add(Instantiate(researchOptionPrefab, transform).GetComponent<ResearchOptionUI>());
            pool[i].DeActivate();
        }
    }
    public void SetListen(Research research, bool listen)
    {
        research.EventListener(Research.ResearchEventType.Finish, AddNextResearch, listen);
        research.EventListener(Research.ResearchEventType.Begin, RemoveAlreadyResearched, listen);

        if (listen)
        {
            Refresh(research.researchOptions);
        }
    }

    private void Refresh(List<ResearchQueueItem> researchQueueItems)
    {
        int length = Mathf.Max(researchQueueItems.Count, researchOptionUIs.Count);
        for (int i = 0; i < length; i++)
        {
            if(i < researchQueueItems.Count && i < researchOptionUIs.Count)
            {
                if(researchQueueItems[i] == researchOptionUIs[i].researchQueueItem)
                {
                    continue;
                }
                else
                {
                    DestroyResearchOption(researchOptionUIs[i]);
                    researchOptionUIs.RemoveAt(i); i--;
                    AddItem(researchQueueItems[i]);
                    continue;
                }
            }
            if(i >= researchQueueItems.Count && i < researchOptionUIs.Count)
            {
                DestroyResearchOption(researchOptionUIs[i]);
                researchOptionUIs.RemoveAt(i); i--;
            }
            if(i < researchQueueItems.Count && i >= researchOptionUIs.Count)
            {
                AddItem(researchQueueItems[i]);
            }
        }
    }

    private void AddNextResearch(Research research, ResearchQueueItem researchQueueItem)
    {
        ResearchQueueItem[] researchQueueItems = researchQueueItem.nextResearch;

        for(int i = 0; i < researchQueueItems.Length; i++)
        {
            AddItem(researchQueueItems[i]);
        }

    }

    private void AddItem(ResearchQueueItem researchQueueItem)
    {
        if(pool.Count > 0)
        {
            pool[pool.Count-1].Initialise(researchQueueItem, colors[(int)researchQueueItem.techType]);
            researchOptionUIs.Add(pool[pool.Count - 1]);
            pool.RemoveAt(pool.Count - 1);
        }
        else
        {
            ResearchOptionUI researchOptionUI = Instantiate(researchOptionPrefab, transform).GetComponent<ResearchOptionUI>();
            researchOptionUI.Initialise(researchQueueItem, colors[(int)researchQueueItem.techType]);
            researchOptionUIs.Add(researchOptionUI);
        }
    }

    private void DestroyResearchOption(ResearchOptionUI researchOptionUI)
    {
        
        if (pool.Count < maxPoolSize)
        {
            researchOptionUI.DeActivate();
            pool.Add(researchOptionUI);
        }
        else
        {
            Destroy(researchOptionUI.gameObject,0.1f);
        }
    }
    private void RemoveItem(ResearchQueueItem researchQueueItem)
    {
        for(int i = 0; i < researchOptionUIs.Count; i++)
        {
            if(researchOptionUIs[i].researchQueueItem == researchQueueItem)
            {
                DestroyResearchOption(researchOptionUIs[i]);
                researchOptionUIs.RemoveAt(i);
                return;
            }
        }
    }

    private void RemoveAlreadyResearched(Research research, ResearchQueueItem researchQueueItem)
    {
        RemoveItem(researchQueueItem);
    }
}
