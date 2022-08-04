using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Economy
{
    //Depenecies
    [System.NonSerialized] private Territory territory;

    //Resource production 
    public Resources resources = new Resources();
    public Resources production = new Resources();
    public int expansionCost = 100;

    //Colony management
    public float PopulationGrowthSpeed = 0.02f;
    public List<BuiltShip> shipTypes;
    public List<BuiltStructure> structureTypes;

    //species: 
    public List<Species> species;

    public virtual void Init(Territory territory)
    {
        this.territory = territory;
        this.production = new Resources(new int[]{ 100, 100, 100, 100 });
    }

    public virtual void AddResources(Resources resources)
    {
        this.resources += resources;
    }

    public virtual void AddProduction(Resources production)
    {
        this.production += production;
    }

    public virtual void SetResourceAmount(ResourceType resourceType, int amount)
    {
        this.resources.amounts[(int)resourceType] = amount;
    }

    public bool Pay(int cost, ResourceType resourceType = ResourceType.MATERIALS)
    {

        if (resources.amounts[(int)resourceType] >= cost)
        {
            resources.amounts[(int)resourceType] -= cost;
            return true;
        }
        return false;
    }

    public bool CanPay(int cost, ResourceType resourceType = ResourceType.MATERIALS)
    {
        return resources.amounts[(int)resourceType] >= cost;
    }

    public void GrowPopulation()
    {
        List<Star> colonyStars = territory.colonyStars;

        for (int i = 0; i < colonyStars.Count; i++)
        {
            List<PlanetColony> colonies = colonyStars[i].starEconomy.colonies;

            for(int j = 0; j < colonies.Count;++j)
            {
                Species specimen = species[Random.Range(0, species.Count)];
                colonies[j].AddPop(specimen);
            }
        }
    }

}
