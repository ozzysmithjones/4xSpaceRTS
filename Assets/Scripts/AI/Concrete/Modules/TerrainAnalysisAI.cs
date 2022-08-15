using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class TerrainAnalysisAI : AIModule
{

    private static Star GetThreatenedStar(Empire empire, Analysis analysis)
    {
        //Find the best defence rendezvous point.

        double greatestThreat = 0.0;
        Star threatened = null;

        foreach (Star star in empire.territory.stars)
        {
            double economy = analysis.allyEconomyMap[star.index];
            double defence = analysis.allyMilitaryMap[star.index];
            double threat = (analysis.enemyMilitaryMap[star.index] - defence) * economy;

            if (threat > greatestThreat)
            {
                threat = greatestThreat;
                threatened = star;
            }
        }

        return threatened;
    }

    protected override void Analyse(Analysis analysis)
    {

        analysis.defendRendezvous.star = GetThreatenedStar(empire, analysis);



        //find the best ambush rendezvous point.


    }

    protected override void Behave(Analysis analysis)
    {

    }

    protected override AIModule CreateCopy()
    {
        TerrainAnalysisAI copy = ScriptableObject.CreateInstance<TerrainAnalysisAI>();
        return copy;
    }

    protected override void OnInit()
    {

    }
}
