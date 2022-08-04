using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildQueueOption", menuName = "AI/Options/BuildQueue")]
public class BuildQueueOption : Option
{
    [HideInInspector] public PlanetColony planetColony;
    public BuildTarget buildTarget = new BuildTarget();

    public void Build(PlanetColony colony)
    {
        colony.AddToBuildQueue(buildTarget.item, 1);
    }

    protected override Option CreateCopy()
    {
        BuildQueueOption copy = ScriptableObject.CreateInstance<BuildQueueOption>();
        copy.buildTarget.item = buildTarget.item;
        return copy;
    }
}

