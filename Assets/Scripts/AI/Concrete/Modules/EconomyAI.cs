
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EconomyAI", menuName = "AI/Modules/Economy")]
public class EconomyAI : AIModule
{
    public BuildQueueOption[] buildQueueOptions = new BuildQueueOption[0];
    public StarOption expandOption;

    private int minBuildCost = int.MinValue;

    protected override AIModule CreateCopy()
    {
        EconomyAI copy = ScriptableObject.CreateInstance<EconomyAI>();

        copy.buildQueueOptions = new BuildQueueOption[this.buildQueueOptions.Length];
        for(int i = 0; i < buildQueueOptions.Length; ++i)
        {
            copy.buildQueueOptions[i] = (BuildQueueOption)buildQueueOptions[i].Clone();
        }

        copy.expandOption = (StarOption)this.expandOption.Clone();
        copy.minBuildCost = this.minBuildCost;
        return copy;
    }

    protected override void OnInit()
    {
        minBuildCost = empire.economy.expansionCost;
        foreach(BuildQueueOption option in buildQueueOptions)
        {
            minBuildCost = Mathf.Min(option.buildTarget.item.buildCost, minBuildCost);
        }
    }

    protected override void Analyse(Analysis analysis)
    {
    }

    protected override void Behave(Analysis analysis)
    {
        if(!empire.economy.CanPay(minBuildCost))
        {
            return;
        }

        GetBestBuildOption(analysis, out PlanetColony buildColony, out BuildQueueOption buildOption, out float buildUtility);
        GetBestExpandOption(analysis, out Star expandStar, out StarOption expandOption, out float expandUtility);

        if(buildUtility >= expandUtility && buildOption != null)
        {
            if (empire.economy.Pay(buildOption.buildTarget.item.buildCost, ResourceType.MATERIALS))
            {
                buildOption.Build(buildColony);
            }

        }else if(expandUtility >= buildUtility && expandOption != null)
        {
            if(empire.economy.Pay(empire.economy.expansionCost, ResourceType.MATERIALS))
            {
                expandOption.Expand(expandStar, empire);
            }
        }
    }

    private void GetBestBuildOption(Analysis analysis, out PlanetColony buildColony, out BuildQueueOption buildOption, out float buildUtility)
    {
        List<Star> colonyStars = empire.territory.colonyStars;
        buildUtility = float.MinValue;
        buildColony = null;
        buildOption = null;

        foreach (Star star in colonyStars)
        {
            foreach (PlanetColony colony in star.starEconomy.colonies)
            {
                if (colony.buildQueue.Count > 0)
                {
                    continue;
                }

                foreach (BuildQueueOption option in buildQueueOptions)
                {
                    option.planetColony = colony;
                    float utility = option.Calculate(option.buildTarget, analysis);

                    if (utility > buildUtility)
                    {
                        buildUtility = utility;
                        buildOption = option;
                        buildColony = colony;
                    }
                }
            }
        }
    }

    private void GetBestExpandOption(Analysis analysis, out Star expandStar, out StarOption expandOption, out float expandUtility)
    {
        List<Star> outerRim = empire.territory.outerRim;
        expandStar = null;
        expandOption = this.expandOption;
        expandUtility = float.MinValue;

        SpatialTarget spatialTarget = new SpatialTarget();
        spatialTarget.star = null;

        foreach (Star star in outerRim)
        {
            spatialTarget.star = star;
            expandOption.star = star;
            float utility = expandOption.Calculate(spatialTarget,analysis);

            if(utility > expandUtility)
            {
                
                expandUtility = utility;
            }
        }

        expandOption.star = expandStar;
    }
}
