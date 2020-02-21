using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Faction
{
    private Text[] resourcesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //randomises this faction.
    public Player(int index = 0, bool random = false, Color[] ColorArray = null, string[] NameArray = null)
    {
        resourcesText = Master.instance.userInterface.resourcesText;

        factionIndex = index;
        if (random)
        {
            flagColor = ColorArray[Random.Range(0, ColorArray.Length)];
            factionName = NameArray[Random.Range(0, NameArray.Length)];

        }
    }


    public override void Gather(Resources resources)
    {
        base.Gather(resources);
        for(int i = 0; i < resourcesText.Length; i++)
        {
            if(resources.amounts[i] == 0)
            {
                continue;
            }
            resourcesText[i].text = ((ResourceType)i).ToString() + ": " + this.resources.amounts[i];
        }
       
    }
}
