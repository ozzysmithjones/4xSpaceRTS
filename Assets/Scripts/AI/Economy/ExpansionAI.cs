using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionAI : SubManager
{
    private Timer expansionTimer;
    public ExpansionAI(AI ai) : base(ai)
    {
        expansionTimer = new Timer(0.2f, Expand);
    }
    //TODO: work out the value of expansion.

    public override float Worth()
    {
        return 100.0f;
    }

    public override void Manage(float deltaTime)
    {
        base.Manage(deltaTime);
        expansionTimer.Tick(deltaTime);
       
    }

    private void Expand()
    {
        
        float highestValue = 0.0f;
        int index = -1;
        for (int i = 0; i < ai.outerRim.Count; i++)
        {
            if (ai.outerRim[i].factionIndex >= 0)
            {
                continue;
            }
            float value = ai.outerRim[i].starEconomy.resourceProduction.amounts[(int)ResourceType.MATERIALS];
            if (value >= highestValue)
            {
                highestValue = value;
                index = i;
            }
        }
        if(index < 0)
        {
            return;
        }
        
        ai.outerRim[index].TakeOver(ai.factionIndex);
    }
}
