using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public PlanetOverview planetOverview;
    public BuildOptionUI[] buildOptions;

    private void Awake()
    {
        for(int i = 0; i < buildOptions.Length; i++)
        {
            buildOptions[i].buildMenu = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(BuildQueueItem buildQueueItem, int amount)
    {
        
        //pay for it first.
        Faction faction = Master.instance.characters.factions[planetOverview.planet.star.factionIndex];
        int price = amount * buildQueueItem.buildCost;

        /*
        //paying for it.
        if(faction.resources.amounts[(int)ResourceType.MATERIALS] < price)
        {
            return;
        }
        Resources costs = new Resources();
        costs.amounts[(int)ResourceType.MATERIALS] = -price;
        faction.Gather(costs);
        */

        //add to build Queue.
        Queue queue = new Queue
        {
            item = buildQueueItem,
            quantity = amount,
            id = Time.time
        };

        // planetOverview.planet.planetColony.BeginBuildQueue();

        planetOverview.planet.planetColony.AddToBuildQueue(queue);
    }

   
}
