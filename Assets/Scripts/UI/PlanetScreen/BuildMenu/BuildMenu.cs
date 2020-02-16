using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    public PlanetOverview planetOverview;
    public BuildOption[] buildOptions;

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
        Faction faction = Master.instance.factions.factions[planetOverview.planet.star.factionIndex];
        int price = amount * buildQueueItem.materialCost;

        /* ignoring prices:
        if(faction.resources[1] < price)
        {
            return;
        }
        faction.Gather(new int[3]{0 ,-price,0});
            
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

        for(int i = 0; i < planetOverview.planet.planetColony.buildQueue.Count; i++)
        {
            
            if(planetOverview.planet.planetColony.buildQueue[i] == planetOverview.planet.planetColony.buildQueue[0] && i != 0)
            {
                print("the same");
            }
        }

    }

   
}
