using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ResourceType
{
    FOOD,
    STABILITY,
    MATERIALS,
    SCIENCE
}
public class Resources 
{
    public int[] amounts;

    public Resources()
    {
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        amounts = new int[length];
    }

    public int Total()
    {
        int amount = 0;
        for (int i = 0; i < amounts.Length; i++)
            amount += amounts[i];
        return amount;
    }
}
