using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetColony : MonoBehaviour
{

    public Planet planet;

    public bool buildQueueRunning = false;
    public List<Queue> buildQueue = new List<Queue>();
    public int buildQueueIndex = -1;

    public int totalStructures = 0;
    public int[] builtStructures = new int[10];
    public delegate void OnBuildQueueChange(List<Queue> buildQueue);
    public event OnBuildQueueChange BuildQueueChange;


    public int totalPopulation = 0;
    public List<Population> populations = new List<Population>();
    public delegate void OnPopulationChange(List<Population> populations);
    public event OnPopulationChange PopulationChange;

    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Colonise(int factionIndex)
    {
        AddPop(Master.instance.characters.factions[factionIndex].species[0].index);
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

        if(builtStructures[builtStructure.classIndex] < 0)
        {
            builtStructures[builtStructure.classIndex] = 0;
        }
        if (Master.instance.userInterface.planetOverviewOpen && Master.instance.userInterface.planetOverview.planet == planet)
        {
            Master.instance.userInterface.planetOverview.alreadyBuilt.UpdateQuantity(builtStructure.classIndex);
        }
    }


    public void AddPop(int speciesIndex)
    {
        totalPopulation++;
        for (int i = 0; i < populations.Count; i++)
        {
            if (populations[i].species.index == speciesIndex)
            {
                populations[i].size++;
                if (PopulationChange != null)
                {
                    PopulationChange.Invoke(populations);
                }
                return;
            }
        }
        populations.Add(new Population(Master.instance.characters.species[speciesIndex], 1, 1.0f));
        if (PopulationChange != null)
        {
            PopulationChange.Invoke(populations);
        }
    }

    public void RemovePop(int speciesIndex)
    {
        for (int i = 0; i < populations.Count; i++)
        {
            if (populations[i].species.index == speciesIndex)
            {
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
        if(buildQueue.Count <= 0)
        {
            //Debug.LogWarning("no items in queue");
            yield break;
        }

        buildQueueRunning = true;
        buildQueue[0].startTime = Time.time;

        while(buildQueue.Count > 0)
        {
            yield return StartCoroutine(BuildSequence(0));
            if(buildQueue.Count <= 0)
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
            if(index >= buildQueue.Count)
            {
                break;
            }

            if(Time.time - buildQueue[index].startTime > buildQueue[index].item.buildTime)
            {
                break;
            }

            yield return null;

        }
    }
    


    public void AddToBuildQueue(Queue queue)
    {
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



    
}

    
