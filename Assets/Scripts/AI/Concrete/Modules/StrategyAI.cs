using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "StrategyAI", menuName = "AI/Modules/Strategy")]
public class StrategyAI : AIModule
{
    protected override AIModule CreateCopy()
    {
        return ScriptableObject.CreateInstance<StrategyAI>();
    }

    protected override void Analyse(Analysis analysis)
    {

    }

    protected override void Behave(Analysis analysis)
    {

    }

    protected override void OnInit()
    {

    }
}
