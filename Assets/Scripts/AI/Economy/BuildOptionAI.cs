using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildOptionAI
{
    public BuildQueueItem buildQueueItem;
    public OptionWorth optionWorth;

    public float GetValue(AI ai)
    {
        return optionWorth.Calculate(ai);
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
