using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Empire
{
    public static Empire player = null;

    //territory control
    public List<Star> colonies = new List<Star>();
    public List<Star> territory = new List<Star>();
    public List<Star> outerRim = new List<Star>();

    //cosmetic
    public string factionName;
    public Color flagColor;

    //building
    public List<BuiltShip> shipTypes;
    public List<BuiltStructure> structureTypes;

    //Economy
    public Resources resources = new Resources();
    public Resources spaceResourceProduction = new Resources();
    protected Resources totalResourceProduction = new Resources();
    public int expansionCost = 100;

    //Research
    public Research research;

    //military:
    public List<Fleet> fleets = new List<Fleet>();
    public List<Empire> enemies = new List<Empire>();

    //species and internal politics: 
    public List<Species> species;

    private Timer PopulationGrowthTimer;
    public float PopulationGrowthSpeed = 0.02f;

    public Empire(Color flagColor, string factionName)
    {
        this.factionName = factionName;
        this.flagColor = flagColor;

        PopulationGrowthTimer = new Timer(1.0f, GrowPopulation);
        research = new Research(this);
    }


    public bool IsEnemyTo(Empire empire)
    {
        return empire != this; //&& enemies.Contains(empire);
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update(float deltaTime)
    {

        PopulationGrowthTimer.endTime = colonies.Count;
        PopulationGrowthTimer.Tick(PopulationGrowthSpeed * deltaTime);

        research.Update();
    }

 


    public void GrowPopulation()
    {
        for (int i = 0; i < colonies.Count; i++)
        {
            colonies[i].starEconomy.colonies[0].AddPop(species[0].index);
        }
    }

    public void AddToTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {

        outerRim.Remove(star);

        if (territory.Contains(star))
        {
            return;
        }
        territory.Add(star);

        if (colony)
        {
            colonies.Add(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (!outerRim.Contains(connectedStars[i]) && connectedStars[i].empire != this)
            {
                outerRim.Add(connectedStars[i]);
            }
        }
    }

    public void RemoveFromTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {
        territory.Remove(star);

        if (colony)
        {
            colonies.Remove(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (outerRim.Contains(connectedStars[i]) && !connectedStars[i].starConnections.IsConnectedToEmpire(this))
            {
                outerRim.Remove(connectedStars[i]);
            }
        }
    }


    public void RandomlyExpand(int lowest = 3, int highest = 8)
    {
        int length = Random.Range(lowest, highest);
        for (int i = 0; i < length; i++)
        {
            if (outerRim.Count <= 0)
            {
                break;
            }
            int index = Random.Range(0, outerRim.Count);

            if (outerRim[index].empire == null)
            {
                outerRim[index].TakeOver(this);
            }
            else
            {
                outerRim.RemoveAt(index);
                i--;
            }

        }
    }

    public virtual void Gather(Resources resources)
    {
        for (int i = 0; i < resources.amounts.Length; i++)
        {
            if (i < this.resources.amounts.Length)
            {
                this.resources.amounts[i] += resources.amounts[i];

                if (this.resources.amounts[i] < 0)
                {
                    this.resources.amounts[i] = 0;
                }
            }
        }

    }

    public virtual void ProduceResources()
    {
        totalResourceProduction.Clear();
        int[] colonyproduction = GetColonyResourceProduction();

        for (int i = 0; i < totalResourceProduction.amounts.Length; i++)
        {
            totalResourceProduction.amounts[i] += colonyproduction[i] + spaceResourceProduction.amounts[i];
        }

        Gather(totalResourceProduction);
    }


    public virtual void SetResourceAmount(ResourceType resourceType, int amount)
    {
        this.resources.amounts[(int)resourceType] = amount;
    }

    public virtual void ModifySpaceResourceProduction(ResourceType resourceType, int amount)
    {
        spaceResourceProduction.amounts[(int)resourceType] += amount;
        totalResourceProduction.amounts[(int)resourceType] += amount;
    }

    public int[] GetColonyResourceProduction()
    {
        int[] totalOutput = new int[spaceResourceProduction.amounts.Length];
        for (int i = 0; i < colonies.Count; i++)
        {
            int[] colonyOutput = colonies[i].starEconomy.GetColonyResourceproduction();
            for (int o = 0; o < totalOutput.Length; o++)
            {
                totalOutput[o] += colonyOutput[o];
            }
        }

        return totalOutput;
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





}
