using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class BuildMenu : MonoBehaviour
{
    public TMP_Text BuildOptionDescription;
    public PlanetOverview planetOverview;
    public BuildOptionUI[] buildOptions;
    public CategoryOverview categoryOverview;

    private void Awake()
    {
        for (int i = 0; i < buildOptions.Length; i++)
        {
            buildOptions[i].buildMenu = this;
        }
    }

    private void OnEnable()
    {
        UpdateOptions();
        categoryOverview.UpdateCategory(BuildQueueItem.Category.Economy, true);
    }

    public void UpdateOptions()
    {
        List<BuiltStructure> builtStructures = Master.instance.characters.empires[0].structureTypes;
        List<BuiltShip> builtShips = Master.instance.characters.empires[0].shipTypes;
        int structureIndex = 0, shipIndex = 0;

        for (int i = 0; i < buildOptions.Length; i++)
        {
            if (structureIndex < builtStructures.Count)
            {
                buildOptions[i].Initialise(builtStructures[structureIndex]);
                structureIndex++;

            }
            else if (shipIndex < builtShips.Count)
            {
                buildOptions[i].Initialise(builtShips[shipIndex]);
                shipIndex++;
            }
            else
            {
                buildOptions[i].Remove();
            }
        }
    }

    

    public void Build(BuildQueueItem buildQueueItem, int amount)
    {
        if(amount <= 0)
        {
            return;
        }
        //pay for it first.
        Empire empire = planetOverview.planet.star.empire;
        int price = amount * buildQueueItem.buildCost;

        /*
        //paying for it.
        if(empire.resources.amounts[(int)ResourceType.MATERIALS] < price)
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
