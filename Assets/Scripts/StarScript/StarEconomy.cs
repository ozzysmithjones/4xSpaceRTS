using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEconomy : MonoBehaviour
{

    //the economy is made up of a few different scripts. The freighter script manages carrying resources to the nearest colony.
    //The mining script manages mining the nearby planets and loading the goods onto the freighters. 
    //this script also has a few values associated with production, which impacts how the star is generated.(e.g energy focused systems
    //have plenty of gas giants)

    public Resources resourceProduction = new Resources();
    private Star star;

    public void StartEconomy()
    {
        
    }

    public void Initialise()
    {
        star = GetComponent<Star>();
    }

    public void ApplyResourceproduction(bool positive)
    {
        if (star.factionIndex < 0)
        {
            return;
        }
        Faction faction = Master.instance.characters.factions[star.factionIndex];

        for (int i = 0; i < resourceProduction.amounts.Length; i++)
        {
            faction.ImproveResourceProduction((ResourceType)i, resourceProduction.amounts[i] * (positive ? 1 : -1));
        }

    }

    public void ModifyResourceProduction(ResourceType resourceType, int amount)
    {
        resourceProduction.amounts[(int)resourceType] += amount;
        Master.instance.characters.factions[star.factionIndex].ImproveResourceProduction(resourceType, amount);
    }
}
