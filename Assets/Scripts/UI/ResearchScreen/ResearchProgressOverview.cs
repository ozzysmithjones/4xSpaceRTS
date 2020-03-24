using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchProgressOverview : MonoBehaviour
{
    public Slider slider;

    public void SetListen(Research research, bool listen)
    {
        research.EventListener(Research.ResearchEventType.Update, UpdateProgress, listen);

        if (listen)
            UpdateProgress(research, research.researchQueueItem);
    }
    
    private void UpdateProgress(Research research,ResearchQueueItem researchQueueItem)
    {
        slider.value = research.Getprogress();
    }
}
