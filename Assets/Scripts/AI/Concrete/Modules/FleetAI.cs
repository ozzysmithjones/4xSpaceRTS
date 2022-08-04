using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Military AI", menuName = "AI/Modules/Fleet AI")]
public class FleetAI : AIModule
{
    protected override void Analyse(Analysis analysis)
    {

    }




    protected override void Behave(Analysis analysis)
    {
        List<Fleet> fleets = empire.military.GetFleets(FleetType.Military);
    }

    protected override AIModule CreateCopy()
    {
        return ScriptableObject.CreateInstance<FleetAI>();
    }

    protected override void OnInit()
    {

    }
}
