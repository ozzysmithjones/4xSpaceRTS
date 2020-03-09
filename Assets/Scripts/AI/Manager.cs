using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager
{
    protected AI ai;
    public Manager(AI ai)
    {
        this.ai = ai;
    }
    public virtual void Update(float deltaTime)
    {
        
    }
    protected void CalculateSubManagers(SubManager[] subManagers)
    {
        for(int i = 0; i < subManagers.Length; i++)
        {
            subManagers[i].Worth();
        }
    }
    
    protected SubManager HighestRatedSubManager(SubManager[] subManagers)
    {
        float highestValue = 0.0f;
        int index = 0;
        for (int i = 0; i < subManagers.Length; i++)
        {
            if (subManagers[i].value >= highestValue)
            {
                highestValue = subManagers[i].value;
                index = i;
            }
        }
        return subManagers[index];
    }
    

   
}
