using UnityEngine;
using UnityEngine.UI;

public class Player : Faction
{
    private Text[] resourcesText;


    //randomises this faction.
    public Player(int index, Color flagColor, string factionName) : base(index, flagColor, factionName)
    {
        resourcesText = Master.instance.userInterface.resourcesText;
    }


    public override void Gather(Resources resources)
    {
        base.Gather(resources);
        for (int i = 0; i < resourcesText.Length; i++)
        {
            if (resources.amounts[i] == 0)
            {
                continue;
            }
            resourcesText[i].text = ResourceText((ResourceType)i);
        }

    }

    public override void ProduceResources()
    {
        base.ProduceResources();

        for(int i = 0; i < totalResourceProduction.amounts.Length; i++)
        {
            ResourceText((ResourceType)i);
        }
       
    }

    private string ResourceText(ResourceType resourceType)
    {
        string production = totalResourceProduction.amounts[(int)resourceType] >= 0 ? "+" + totalResourceProduction.amounts[(int)resourceType] : "-" + -totalResourceProduction.amounts[(int)resourceType];
        return resourceType.ToString() + ": " + this.resources.amounts[(int)resourceType] + " " + production;
    }
}
