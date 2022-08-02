using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColonyAI : AIModule
{
    public PathOption coloniseOption;

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

    private Planet FindBestPLanet(Fleet fleet, Analysis analysis)
    {
        Planet bestPlanet = null;
        float highestUtility = float.MinValue;

        List<Star> stars = empire.territory.stars;
        for (int i = 0; i < stars.Count; ++i)
        {
            Planet[] planets = stars[i].starGeneration.planets;

            for (int j = 0; j < planets.Length; ++j)
            {
                if (planets[i].isColony)
                {
                    continue;
                }

                coloniseOption.planet = planets[i];
                coloniseOption.star = stars[i];
                coloniseOption.fleet = fleet;
                float utility = coloniseOption.Calculate(analysis);

                if (utility > highestUtility)
                {
                    highestUtility = utility;
                    bestPlanet = planets[i];
                }
            }
        }

        return bestPlanet;
    }

    protected override void OnBehave(Analysis analysis)
    {
        List<Fleet> colonyFleets = empire.military.GetFleets(FleetType.Colony);

        for (int i = 0; i < colonyFleets.Count; ++i)
        {
            if (!colonyFleets[i].HasOrders() && !colonyFleets[i].Busy())
            {
                Planet bestPlanet = FindBestPLanet(colonyFleets[i], analysis);
                colonyFleets[i].AddOrder(new ColoniseOrder(bestPlanet));
            }
        }
    }
}
