using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyAI : SubManager
{
    public SubManager[] subManagers;
    public ColonyAI(AI ai) : base(ai)
    {
        subManagers = new SubManager[2];
        subManagers[0] = new ColonyStructureAI(ai);
        subManagers[1] = new ColonyMilitaryAI(ai);
    }
    public override float Worth()
    {
        CalculateSubManagers(subManagers);
        value = HighestRatedSubManager(subManagers).value;
        return value;
    }

    public override void Manage(float deltaTime)
    {
        base.Manage(deltaTime);
        HighestRatedSubManager(subManagers).Manage(deltaTime);
    }
}
