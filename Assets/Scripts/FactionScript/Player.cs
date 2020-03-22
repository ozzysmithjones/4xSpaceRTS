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

    public override void ImproveResourceProduction(ResourceType resourceType, int amount)
    {
        base.ImproveResourceProduction(resourceType, amount);

        resourcesText[(int)resourceType].text = ResourceText(resourceType);
    }

    private string ResourceText(ResourceType resourceType)
    {
        string production = resourceProduction.amounts[(int)resourceType] >= 0 ? "+" + resourceProduction.amounts[(int)resourceType] : "-" + -resourceProduction.amounts[(int)resourceType];
        return resourceType.ToString() + ": " + this.resources.amounts[(int)resourceType] + " " + production;
    }
}
