using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOptionAI
{
    public BuildQueueItem buildQueueItem;
    public Worth optionWorth;

    public float GetValue()
    {
        return optionWorth.Calculate();
    }


    public void Build(int quantity, PlanetColony planetColony)
    {
        //add to build Queue.
        Queue queue = new Queue
        {
            item = buildQueueItem,
            quantity = quantity,
            id = Time.time
        };
        planetColony.AddToBuildQueue(queue);
    }

}
