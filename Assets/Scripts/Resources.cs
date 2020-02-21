using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    ENERGY,
    MATERIALS,
    DEATHMATTER
}

public class Resources 
{
    public int[] amounts = new int[3];

    /* an example showing how resources are set and gotten. 
    public void SetAmount(ResourceType resourceType, int amount)
    {
        amounts[(int)resourceType] = amount;
    }

    public int GetAmount(ResourceType resourceType)
    {
        return amounts[(int)resourceType];
    }
    */



}
