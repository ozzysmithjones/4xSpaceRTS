using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    ENERGY,
    MATERIALS,
    DEATHMATTER
}

[System.Serializable]
public class Resources 
{
    public int[] amounts = new int[3];

    public int Total()
    {
        int amount = 0;
        for (int i = 0; i < amounts.Length; i++)
            amount += amounts[i];
        return amount;
    }
}
