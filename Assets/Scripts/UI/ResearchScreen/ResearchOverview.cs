using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchOverview : MonoBehaviour
{
    private Research research;
    public CurrentResearchOverview currentResearchOverview;
    public ResearchProgressOverview researchProgressOverview;
    public ResearchOptionsOverview researchOptionsOverview;

    public void Overview(Research research)
    {
        this.research = research;
        researchProgressOverview.SetListen(research, true);
        currentResearchOverview.SetListen(research, true);
        researchOptionsOverview.SetListen(research, true);
    }


    public void Open(Research research)
    {
        Master.instance.userInterface.planetOverviewOpen = true;
        gameObject.SetActive(true);
        Overview(research);

    }

    public void Close()
    {
        if(research != null)
        {
            researchProgressOverview.SetListen(research, false);
            currentResearchOverview.SetListen(research, false);
            researchOptionsOverview.SetListen(research, false);
        }
        gameObject.SetActive(false);
    }
}
