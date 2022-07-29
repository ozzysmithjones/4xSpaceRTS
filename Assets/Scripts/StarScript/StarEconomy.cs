using System.Collections.Generic;
using UnityEngine;

public class StarEconomy : MonoBehaviour
{
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
        if (star.empire == null)
        {
            return;
        }

        for (int i = 0; i < totalResourceProduction.amounts.Length; i++)
        {
            star.empire.ModifySpaceResourceProduction((ResourceType)i, totalResourceProduction.amounts[i] * (positive ? 1 : -1));
        }

    }

    public void ModifyTotalResourceProduction(ResourceType resourceType, int amount)
    {
        totalResourceProduction.amounts[(int)resourceType] += amount;
        star.empire.ModifySpaceResourceProduction(resourceType, amount);
    }

    public int[] GetColonyResourceproduction()
    {
        int[] TotalOutput = new int[totalResourceProduction.amounts.Length];

        for(int i = 0; i < colonies.Count; i++)
        {
            int[] colonyOuput = colonies[i].ProduceResources();
            for(int o = 0; o < colonyOuput.Length; o++)
            {
                TotalOutput[o] += colonyOuput[o];
            }
        }

        return TotalOutput;
    }
}
