using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyAI : AIModule
{
    protected override AIModule CreateCopy()
    {
        return ScriptableObject.CreateInstance<ColonyAI>();
    }

    protected override void OnInit()
    {

    }

    protected override void OnAnalyse(Analysis analysis)
    {

    }

    protected override void OnBehave(Analysis analysis)
    {

    }
}
