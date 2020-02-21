using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factions : MonoBehaviour
{
    
    public List<Faction> factions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFactions(BuiltShip[] shipTypes, BuiltStructure[] structureTypes)
    {
        factions.Add(CreateFaction(0,shipTypes,structureTypes,true));

        //player home world:
        //Star playerStar = Master.instance.enviroment.RandomStar();
        //playerStar.isColony = true;
        //playerStar.TakeOver(0);


        for(int i = 0; i < 5;i++)
        {
            factions.Add(CreateFaction(i,shipTypes, structureTypes));
        }
        /*
        //AI home worlds:
        for (int i = 1; i < 5; i++)
        {
            factions.Add(CreateFaction(i));
            Star star = Master.instance.enviroment.RandomStar();
            star.isColony = true;
            star.TakeOver(i);
        }
        //random expansion:
        for(int i = 0; i < factions.Count; i++)
        {
            
            factions[i].RandomlyExpand();
        }
        */
    }

    public Faction CreateFaction(int index, BuiltShip[] shipTypes, BuiltStructure[] structureTypes,bool player = false)
    {

        Color[] colors = new Color[1];
        colors[0] = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        string[] names = new string[1];
        names[0] = "bob";

        Faction instance;

        if (!player)
        {
            instance = new Faction(index, true, colors, names);
        }
        else
        {
            instance = new Player(index, true, colors, names);
        }

        instance.structureTypes = structureTypes;
        instance.shipTypes = shipTypes;
        
        return instance;
    }
}
