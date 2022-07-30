using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetColony : MonoBehaviour
{

    public Planet planet;

    public bool buildQueueRunning = false;
    public List<BuildQueueElement> buildQueue = new List<BuildQueueElement>();
    public int buildQueueIndex = -1;

    public int totalStructures = 0;
    public int[] builtStructures = new int[10];
    public delegate void OnBuildQueueChange(List<BuildQueueElement> buildQueue);
    public event OnBuildQueueChange BuildQueueChange;

    public int totalPopulation = 0;
    public List<Population> populations = new List<Population>();
    public delegate void OnPopulationChange(List<Population> populations);
    public event OnPopulationChange PopulationChange;

    public Resources resourceProduction = new Resources();
    public Resources resourceBonus;

    void Start()
    {
        resourceBonus.amounts = planet.biome.GetRandomProductionAmounts();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddToBuildQueue(BuildQueueItem buildQueueItem, int amount = 1)
    {
        //add to build Queue.
        BuildQueueElement queue = new BuildQueueElement
        {
            item = buildQueueItem,
            quantity = amount,
        };

        buildQueue.Add(queue);

        if (!buildQueueRunning)
        {
            StartCoroutine(BeginBuildQueue());

        }
        if (BuildQueueChange != null)
        {
            BuildQueueChange.Invoke(buildQueue);
        }
    }

    public void Colonise(Empire empire)
    {
        AddPop(empire.economy.species[0]);
        planet.star.starEconomy.colonies.Add(this);
    }

    public void ListenToPopulation(OnPopulationChange onPopulationChange, bool listening)
    {
        if (listening)
        {
            PopulationChange += onPopulationChange;

        }
        else
        {
            PopulationChange -= onPopulationChange;
        }
    }

    public void ListenToBuildQueue(OnBuildQueueChange onBuildQueueChange, bool listening)
    {
        if (listening)
        {
            BuildQueueChange += onBuildQueueChange;

        }
        else
        {
            BuildQueueChange -= onBuildQueueChange;
        }
    }

    public void AddStructure(BuiltStructure builtStructure)
    {
        totalStructures++;
        builtStructures[builtStructure.classIndex]++;

        if (Master.instance.userInterface.planetOverviewOpen && Master.instance.userInterface.planetOverview.planet == planet)
        {
            Master.instance.userInterface.planetOverview.alreadyBuilt.UpdateQuantity(builtStructure.classIndex);
        }

    }


    public void RemoveStructure(BuiltStructure builtStructure)
    {
        totalStructures--;
        builtStructures[builtStructure.classIndex]--;

        if (builtStructures[builtStructure.classIndex] < 0)
        {
            builtStructures[builtStructure.classIndex] = 0;
        }
        if (Master.instance.userInterface.planetOverviewOpen && Master.instance.userInterface.planetOverview.planet == planet)
        {
            Master.instance.userInterface.planetOverview.alreadyBuilt.UpdateQuantity(builtStructure.classIndex);
        }
    }


    public void AddPop(Species species)
    {
        Resources previous = Mod(resourceProduction.Clone());
        AddNewPop(species);
        Resources diff = Mod(resourceProduction - previous);

        if (planet.star.empire != null)
        {
            planet.star.empire.economy.AddProduction(diff);
        }
    }

    private void AddNewPop(Species species)
    {
        resourceProduction.amounts[(int)ResourceType.FOOD]--;
        totalPopulation++;

        for (int i = 0; i < populations.Count; i++)
        {
            if (populations[i].species == species)
            {
                populations[i].size++;
                if (PopulationChange != null)
                {
                    PopulationChange.Invoke(populations);
                }

                return;
            }
        }

        populations.Add(new Population(species, 1, 1.0f));

        if (PopulationChange != null)
        {
            PopulationChange.Invoke(populations);
        }
    }

    public void RemovePop(Species species)
    {
        Resources previous = Mod(resourceProduction.Clone());
        RemoveOldPop(species);
        Resources diff = Mod(resourceProduction - previous);

        if (planet.star.empire != null)
        {
            planet.star.empire.economy.AddProduction(diff);
        }
    }

    private void RemoveOldPop(Species species)
    {
        for (int i = 0; i < populations.Count; i++)
        {
            if (populations[i].species == species)
            {
                resourceProduction.amounts[(int)ResourceType.FOOD]++;
                totalPopulation--;
                populations[i].size--;

                if (PopulationChange != null)
                {
                    PopulationChange.Invoke(populations);
                }
                if (populations[i].size <= 0)
                {
                    populations.RemoveAt(i);
                }

                return;
            }
        }
    }

    public IEnumerator BeginBuildQueue()
    {
        if (buildQueue.Count <= 0)
        {
            //Debug.LogWarning("no items in queue");
            yield break;
        }

        buildQueueRunning = true;
        buildQueue[0].startTime = Time.time;

        while (buildQueue.Count > 0)
        {

            yield return StartCoroutine(BuildSequence(0));
            if (buildQueue.Count <= 0)
            {
                break;
            }

            buildQueue[0].item.Build(planet);
            buildQueue[0].quantity--;

            if (buildQueue[0].quantity <= 0)
            {

                buildQueue.RemoveAt(0);
                if (buildQueue.Count > 0)
                {
                    buildQueue[0].startTime = Time.time;
                }
            }
            else
            {
                buildQueue[0].startTime = Time.time;
                // i--;
            }

            if (BuildQueueChange != null)
            {
                BuildQueueChange.Invoke(buildQueue);
            }
        }
        buildQueueRunning = false;
        buildQueueIndex = -1;
        yield break;
    }

    private IEnumerator BuildSequence(int index)
    {

        buildQueue[index].startTime = Time.time;
        while (true)
        {
            if (index >= buildQueue.Count)
            {
                break;
            }

            if (Time.time - buildQueue[index].startTime > buildQueue[index].item.buildTime)
            {
                break;
            }

            yield return null;

        }
    }

   

    public void RemoveFromBuildQueue(int itemIndex, bool all)
    {
        if (all)
        {
            buildQueue[itemIndex].quantity = 0;
        }
        else
        {
            buildQueue[itemIndex].quantity--;
        }
        if (buildQueue[itemIndex].quantity <= 0)
        {
            // StopCoroutine(BeginBuildQueue());

            buildQueue.RemoveAt(itemIndex);
            if (buildQueue.Count > 0 && itemIndex < buildQueue.Count)
            {
                buildQueue[itemIndex].startTime = Time.time;
            }

            // StartCoroutine(BeginBuildQueue());
        }

        BuildQueueChange.Invoke(buildQueue);

    }

    public void ModifyResourceProduction(ResourceType resourceType, int amount)
    {
        Resources previous = Mod(resourceProduction.Clone());
        resourceProduction.amounts[(int)resourceType] += amount;
        Resources diff = Mod(resourceProduction - previous);
        
        if(planet.star.empire != null)
        {
            planet.star.empire.economy.AddProduction(diff);
        }
    }

    Resources Mod(Resources production)
    {
        if(totalStructures == 0)
        {
            return production * 0;
        }

        float JobFill = Mathf.Clamp01(totalPopulation / (float)totalStructures);
        return  (production * JobFill) + (resourceBonus * totalPopulation);
    }

}


