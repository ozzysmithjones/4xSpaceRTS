using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResearchQueueItem", menuName = "Research/UnlockBuild")]
public class UnlockBuild : ResearchQueueItem
{
    public bool isShip = false;
    public BuildQueueItem buildQueueItem;
    public override void FinishResearch(Empire faction)
    {
        base.FinishResearch(faction);
        if (isShip)
        {
            faction.shipTypes.Add(buildQueueItem as BuiltShip);
        }
        else
        {
            faction.structureTypes.Add(buildQueueItem as BuiltStructure);
        }
    }
}
