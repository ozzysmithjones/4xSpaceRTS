using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ResearchOptionUI : MonoBehaviour, IPointerDownHandler
{
    public ResearchQueueItem researchQueueItem;
    public TMP_Text title;
    public ToolTip toolTip;
    public Image image;
    public Image backGround;

    public void Initialise(ResearchQueueItem researchQueueItem,Color backgroundColor)
    {
        
        title.text = researchQueueItem.name;
        toolTip.SetText(researchQueueItem.description);
        image.sprite = researchQueueItem.sprite;
        backGround.color = backgroundColor;

        this.researchQueueItem = researchQueueItem;
        gameObject.SetActive(true);
    }

    public void DeActivate()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Master.instance.characters.factions[0].research.BeginResearch(researchQueueItem);
    }
}
