using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchQueueItem", menuName = "Research/UnlockBuild")]
public class UnlockBuild : ResearchQueueItem
{
    public bool isShip = false;
    public BuildQueueItem buildQueueItem;
    public override void FinishResearch(Empire empire)
    {
        base.FinishResearch(empire);
        if (isShip)
        {
            empire.economy.shipTypes.Add(buildQueueItem as BuiltShip);
        }
        else
        {
            empire.economy.structureTypes.Add(buildQueueItem as BuiltStructure);
        }
    }
}
