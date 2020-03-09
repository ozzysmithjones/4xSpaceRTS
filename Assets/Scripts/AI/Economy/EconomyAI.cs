using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyAI : Manager
{
    private SubManager[] subManagers;

    public EconomyAI(AI ai) : base(ai)
    {
        subManagers = new SubManager[2];
        subManagers[0] = new ExpansionAI(ai);
        subManagers[1] = new ColonyAI(ai);

    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        CalculateSubManagers(subManagers);

        string text = "";
        for(int i = 0; i < subManagers.Length; i++)
        {
            text += subManagers[i].value.ToString() + ",";
        }
        Debug.Log(text);

        HighestRatedSubManager(subManagers).Manage(deltaTime);
       
    }
}
