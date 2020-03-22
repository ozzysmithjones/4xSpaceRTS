using System.Collections.Generic;
using UnityEngine;

public class StarEconomy : MonoBehaviour
{

    //the economy is made up of a few different scripts. The freighter script manages carrying resources to the nearest colony.
    //The mining script manages mining the nearby planets and loading the goods onto the freighters. 
    //this script also has a few values associated with production, which impacts how the star is generated.(e.g energy focused systems
    //have plenty of gas giants)
    public List<PlanetColony> colonies = new List<PlanetColony>();

    public Resources totalResourceProduction = new Resources();
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

        for (int i = 0; i < totalResourceProduction.amounts.Length; i++)
        {
            faction.ImproveResourceProduction((ResourceType)i, totalResourceProduction.amounts[i] * (positive ? 1 : -1));
        }

    }

    public void ModifyResourceProduction(ResourceType resourceType, int amount)
    {
        totalResourceProduction.amounts[(int)resourceType] += amount;
        Master.instance.characters.factions[star.factionIndex].ImproveResourceProduction(resourceType, amount);
    }
}
