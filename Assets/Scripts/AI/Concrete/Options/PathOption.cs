using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIFleetMoveOption", menuName = "AI/Options/FleetMove")]
public class PathOption : StarOption
{
    [HideInInspector] public Fleet fleet;
    [HideInInspector] public Planet planet;

    protected override Option CreateCopy()
    {
        return ScriptableObject.CreateInstance<PathOption>();
    }
}
