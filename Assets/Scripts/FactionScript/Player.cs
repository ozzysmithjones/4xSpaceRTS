using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Faction
{
    private Text[] resourcesText;


    //randomises this faction.
    public Player(int index, Color flagColor, string factionName) : base(index,flagColor,factionName)
    {
        resourcesText = Master.instance.userInterface.resourcesText;
    }


    public override void Gather(Resources resources)
    {
        base.Gather(resources);
        for(int i = 0; i < resourcesText.Length; i++)
        {
            if(resources.amounts[i] == 0)
            {
                continue;
            }
            resourcesText[i].text = ((ResourceType)i).ToString() + ": " + this.resources.amounts[i] + " +" + this.resourceProduction.amounts[i];
        }
       
    }

    public override void ImproveResourceProduction(ResourceType resourceType, int amount)
    {
        base.ImproveResourceProduction(resourceType, amount);
        string pos = resourceProduction.amounts[(int)resourceType] > 0 ? " +" : " -";
        resourcesText[(int)resourceType].text = resourceType.ToString() + ": " + this.resources.amounts[(int)resourceType] + pos + this.resourceProduction.amounts[(int)resourceType];
    }
}
