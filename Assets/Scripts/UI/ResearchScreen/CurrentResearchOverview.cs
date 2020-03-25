using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CurrentResearchOverview : MonoBehaviour
{
    public TMP_Text title;
    public TMP_Text description;
    public Image image;
    private Sprite defaultSprite;

    private void Start()
    {
        defaultSprite = image.sprite;
    }

    public void SetListen(Research research, bool listen)
    {
        research.EventListener(Research.ResearchEventType.Begin, UpdateCurrentResearch, listen);
        research.EventListener(Research.ResearchEventType.Stop, ClearCurrentResearch, listen);

        
        if (listen && research.researchQueueItem != null)
        {
            UpdateCurrentResearch(research, research.researchQueueItem);
        }
        else
        {
            ClearCurrentResearch(research, research.researchQueueItem);
        }
    }
    private void UpdateCurrentResearch(Research research, ResearchQueueItem researchQueueItem)
    {

        title.text = researchQueueItem.name;
        description.text = researchQueueItem.description;
        image.sprite = researchQueueItem.sprite;
    }
    private void ClearCurrentResearch(Research research, ResearchQueueItem researchQueueItem)
    {
        title.text = "NOTHING";
        description.text = "choose something to research";
        image.sprite = defaultSprite;
    }
}
