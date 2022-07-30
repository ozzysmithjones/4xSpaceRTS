using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildQueueOption", menuName = "AI/Options/BuildQueue")]
public class BuildQueueOption : Option
{
    public PlanetColony planetColony;
    public BuildQueueItem item;

    public void Build(PlanetColony colony)
    {
        colony.AddToBuildQueue(item, 1);
    }

    protected override Option CreateCopy()
    {
        BuildQueueOption copy = ScriptableObject.CreateInstance<BuildQueueOption>();
        copy.item = this.item;
        return copy;
    }
}

