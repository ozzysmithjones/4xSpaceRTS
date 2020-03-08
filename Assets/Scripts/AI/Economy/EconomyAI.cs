using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyAI : Manager
{
    public SubManager[] subManagers;

    public EconomyAI(AI ai) : base(ai)
    {
        subManagers = new SubManager[2];
        subManagers[0] = new ExpansionAI(ai);
        subManagers[1] = new ColonyAI(ai);
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        HighestRatedSubManager(subManagers).Manage(deltaTime);
       
    }
}
