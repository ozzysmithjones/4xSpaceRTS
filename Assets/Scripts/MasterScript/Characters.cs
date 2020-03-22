using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    public const int totalFactions = 3;
    public Species[] species;
    public PoliticalGroup[] politicalGroups;
    public List<Faction> factions;
    public Weight[] weights;

    // Start is called before the first frame update
    void Start()
    {
        weights = UnityEngine.Resources.LoadAll<Weight>("Weights");
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].Initialise(totalFactions);
        }
        StartCoroutine(ResourceProduction());
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < factions.Count; i++)
        {
            factions[i].Update(Time.deltaTime);
        }
    }



    public void SpawnFactions(BuiltShip[] shipTypes, BuiltStructure[] structureTypes)
    {
        factions.Add(CreateFaction(0, shipTypes, structureTypes, true));

        Star star = Master.instance.enviroment.RandomStar();
        star.TakeOver(0);
        star.Colonise(0);



        for (int i = 1; i < totalFactions - 1; i++)
        {
            factions.Add(CreateFaction(i, shipTypes, structureTypes));

            star = Master.instance.enviroment.RandomStar();
            star.TakeOver(i);
            star.Colonise(i, 0);

        }
        for (int i = 0; i < factions.Count; i++)
        {
            factions[i].Start();
        }
        //random expansion:
        //for (int i = 0; i < factions.Count; i++)
        //   {
        // factions[i].RandomlyExpand();
        //  }

    }

    public Faction CreateFaction(int index, BuiltShip[] shipTypes, BuiltStructure[] structureTypes, bool player = false)
    {

        Color[] colors = new Color[1];
        colors[0] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        string[] names = new string[1];
        names[0] = "bob";

        Faction instance;

        if (!player)
        {
            instance = new AI(index, colors[0], names[0]);
        }
        else
        {
            instance = new Player(index, colors[0], names[0]);
        }

        instance.structureTypes = structureTypes;
        instance.shipTypes = shipTypes;

        instance.species = new List<Species>();
        instance.species.Add(species[0]);

        return instance;
    }

    public Weight FindWeight(string name)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i].name == name)
            {
                return weights[i];
            }


        }
        Debug.LogError("couldn't find the AI weight: " + name);
        return null;
    }

    private IEnumerator ResourceProduction()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < factions.Count; i++)
            {
                factions[i].ProduceResources();
            }
        }

    }

}
