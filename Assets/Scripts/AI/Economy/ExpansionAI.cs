using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionAI : SubManager
{
    public ExpansionAI(AI ai) : base(ai)
    {
    }
    //TODO: work out the value of expansion.

    public override float Worth()
    {
        value = HighestStarValue(out int index);
        return value;
    }

    public override void Manage(float deltaTime)
    {
        base.Manage(deltaTime);
        Expand();
       
    }

    private void Expand()
    {

        if (!ai.Pay(100))
        {
            return;
        }
        HighestStarValue(out int index);
        ai.outerRim[index].TakeOver(ai.factionIndex);
    }

    public float HighestStarValue(out int index)
    {
        float highestValue = 0.0f;
        index = -1;
        for (int i = 0; i < ai.outerRim.Count; i++)
        {
            if (ai.outerRim[i].factionIndex >= 0)
            {
                continue;
            }
            float value = ai.outerRim[i].starEconomy.resourceProduction.Total();
            if (value >= highestValue)
            {
                highestValue = value;
                index = i;
            }
        }

        return highestValue;
    }
}
