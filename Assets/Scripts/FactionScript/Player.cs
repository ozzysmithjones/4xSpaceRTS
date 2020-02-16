using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Faction
{
    private Text energyText;
    private Text materialsText;
    private Text deathMatterText;

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
        energyText = Master.instance.userInterface.EnergyText;
        materialsText = Master.instance.userInterface.Materialstext;
        deathMatterText = Master.instance.userInterface.DeathMatterText;
        factionIndex = index;
        if (random)
        {
            flagColor = ColorArray[Random.Range(0, ColorArray.Length)];
            factionName = NameArray[Random.Range(0, NameArray.Length)];

        }
    }


    public override void Gather(int[] produced)
    {
        base.Gather(produced);

        energyText.text = "Energy: " + resources[0];
        materialsText.text = "Materials: " + resources[1];
        deathMatterText.text = "DeathMatter: " + resources[2];
    }
}
