using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetColony : MonoBehaviour
{



    public List<Queue> buildQueue = new List<Queue>();
    public int buildQueueIndex = -1;
    public Planet planet;

    //public List<BuiltStructure> builtStructures = new List<BuiltStructure>();
    public int[] builtStructures = new int[10];


    //the speed at which new buildings and ships are built.
    public float manufactoring = 1f;

    //how much the colony produces:
    private float productionTime = 60f;
    public int[] production = new int[3];

    public bool buildQueueRunning = false;

    public delegate void BuildQueueChange(int changeIndex, bool added,List<Queue> buildQueue);
    public event BuildQueueChange OnBuildQueueChange;
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Colonise()
    {
        StartCoroutine(BeginResourceProduction());
    }

    public void AddStructure(BuiltStructure builtStructure)
    {
        
        builtStructures[builtStructure.classIndex]++;

        if (Master.instance.userInterface.planetOverviewOpen && Master.instance.userInterface.planetOverview.planet == planet)
        {
            Master.instance.userInterface.planetOverview.alreadyBuilt.UpdateQuantity(builtStructure.classIndex);
        }
        {

        }
        /*
        if (builtStructures.ContainsKey(builtStructure))
        {
            builtStructures[builtStructure]++;
        }
        else
        {
            builtStructures.Add(builtStructure, 1);
        }
        */
        
    }

    public void ListenToBuildQueue(BuildQueueChange buildQueueChange, bool listening)
    {
        if (listening)
        {
            OnBuildQueueChange += buildQueueChange;
            Debug.Log("added listerner");
        }
        else
        {
            OnBuildQueueChange -= buildQueueChange;
            Debug.Log("removed listerner");
        }
    }

    public void RemoveStructure(BuiltStructure builtStructure)
    {
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

    
    public IEnumerator BeginBuildQueue()
    {
        if(buildQueue.Count <= 0)
        {
            //Debug.LogWarning("no items in queue");
            yield break;
        }

        buildQueueRunning = true;

        //buildQueueIndex = 0;

        buildQueue[0].startTime = Time.time;

        while(buildQueue.Count > 0)
        //for (; ; )
        {

            //buildQueueIndex = i;

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

            if (OnBuildQueueChange != null)
            {
                OnBuildQueueChange.Invoke(0,false,buildQueue);
            }
        }
        //buildQueue.Clear();
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
    

    private IEnumerator BeginResourceProduction()
    {
        for (;;)
        {
            yield return new WaitForSeconds(productionTime);
            Master.instance.factions.factions[planet.star.factionIndex].Gather(production);
           
        }
    }

    public void AddToBuildQueue(Queue queue)
    {
        buildQueue.Add(queue);
        if (!buildQueueRunning)
        {
            StartCoroutine(BeginBuildQueue());
            
        }
        OnBuildQueueChange.Invoke(buildQueue.Count-1,true,buildQueue);
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

        OnBuildQueueChange.Invoke(itemIndex,false,buildQueue);

    }


    
}

    
