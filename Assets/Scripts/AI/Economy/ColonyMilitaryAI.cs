using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyMilitaryAI : SubManager
{
    private BuildOption[] buildOptions;
    public ColonyMilitaryAI(AI ai) : base(ai)
    {
        buildOptions = UnityEngine.Resources.LoadAll<BuildOption>("Weights/BuildQueueItems/SpaceShips");
    }

    public override float Worth()
    {
        if(ai.resources.amounts[(int)ResourceType.MATERIALS] < 100){
            return 0.0f;
        }
        value = (Calculation.InvasionEconomic(ai, Master.instance.factions.factions[0]) + Calculation.InvasionEconomic(Master.instance.factions.factions[0],ai)) / 2.0f;
       // Debug.Log(value);
        return value;
    }

    public override void Manage(float deltaTime)
    {
        base.Manage(deltaTime);

        int amount = ai.resources.amounts[(int)ResourceType.MATERIALS] / 100;
        if(amount <= 0)
        {
            return;
        }

        for(int i = 0; i < ai.colonies.Count; i++)
        {
            if (ai.Pay(100 * amount))
            {
                Queue queue = new Queue
                {
                    item = buildOptions[0].buildQueueItem,
                    startTime = 0.0f,
                    quantity = amount,
                    id = Time.time
                };

                ai.colonies[i].starGeneration.planets[0].planetColony.AddToBuildQueue(queue);
            }
            else
            {
                return;
            }
        }
    }

}
