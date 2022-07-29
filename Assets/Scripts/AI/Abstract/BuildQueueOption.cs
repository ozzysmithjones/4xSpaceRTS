using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildQueueOption", menuName = "AI/Options/BuildQueue")]
public class BuildQueueOption : Option
{
    public BuildQueueItem item;

    public void Build(PlanetColony colony)
    {
        colony.AddToBuildQueue(item, 1);
    }
}

