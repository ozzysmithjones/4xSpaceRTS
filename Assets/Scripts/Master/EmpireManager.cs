using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpireManager : MonoBehaviour
{
    public ResearchQueueItem startingResearch;
    public const int numEmpires = 3;
    public Species[] species;
    public PoliticalGroup[] politicalGroups;
    public List<Empire> empires;

    public AIModule aiCore;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ResourceProduction());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < empires.Count; i++)
        {
            empires[i].Update(Time.deltaTime);
        }
    }



    public void SpawnFactions(BuiltShip[] shipTypes, BuiltStructure[] structureTypes)
    {
        Empire.player = CreateFaction(shipTypes, structureTypes, true);
        empires.Add(Empire.player);

        Star star = Master.instance.enviroment.RandomStar();
        star.TakeOver(Empire.player);
        star.Colonise(Empire.player);
        for (int i = 1; i < numEmpires - 1; i++)
        {
            Empire empire = CreateFaction(shipTypes, structureTypes);
            empires.Add(empire);

            star = Master.instance.enviroment.RandomStar();
            star.TakeOver(empire);
            star.Colonise(empire, 0);

        }
        for (int i = 0; i < empires.Count; i++)
        {
            empires[i].Start();
        }

        empires[0].research.BeginResearch(startingResearch);
    }

    public Empire CreateFaction(BuiltShip[] shipTypes, BuiltStructure[] structureTypes, bool player = false)
    {
        Color[] colors = new Color[1];
        colors[0] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        string[] names = new string[1];
        names[0] = "bob";

        Empire instance;

        if (!player)
        {
            instance = new AI(colors[0], names[0], aiCore.Clone());
        }
        else
        {
            instance = new Player(colors[0], names[0]);
        }

        instance.economy.structureTypes = new List<BuiltStructure>(structureTypes);
        instance.economy.shipTypes = new List<BuiltShip>(shipTypes);

        instance.economy.species = new List<Species>();
        instance.economy.species.Add(species[0]);

        return instance;
    }

    private IEnumerator ResourceProduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(20.0f);
            for (int i = 0; i < empires.Count; i++)
            {
                empires[i].economy.AddResources(empires[i].economy.production);
            }
        }
    }

}
