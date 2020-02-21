using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Faction  
{
    //the territory owned by the faction.(some wanderer factions may not own any territory.)
    public List<Star> Colonies = new List<Star>();
    public List<Star> territory = new List<Star>();
    public List<Star> outerRim = new List<Star>();

    //the colour of this faction.
    public string factionName;
    public Color flagColor;

    //this integer indicates which index this faction is in the array.
    public int factionIndex;

    private int randomExpansion = 5;


    /*
    public int energy = 0;
    public int materials = 0;
    public int deathMatter = 0;
    */
    public Resources resources = new Resources();

    public BiomeGradient.Point homePlanet = new BiomeGradient.Point(0f, "Home World", Color.green, 0, 0);

    //randomises this faction.
    public Faction(int index = 0,bool random = false,Color[] ColorArray = null,string[] NameArray = null)
    {
        factionIndex = index;
        if (random)
        {
            flagColor = ColorArray[Random.Range(0, ColorArray.Length)];
            factionName = NameArray[Random.Range(0, NameArray.Length)];

        }
    }

    
    public void Influence(Star star, bool showOuterRim = false, bool colony = false)
    {
        if (territory.Contains(star))
        {
            return;
        }
        outerRim.Remove(star);

        territory.Add(star);
        if (colony)
        {
            Colonies.Add(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();
        for(int i = 0; i < connectedStars.Count; i++)
        {
            if (!territory.Contains(connectedStars[i]) && !outerRim.Contains(connectedStars[i]))
            {
                outerRim.Add(connectedStars[i]);
                if (showOuterRim && connectedStars[i].factionIndex < 0)
                {
                    connectedStars[i].SetSelector(true, Color.white);
                }
            }
        }

    }

    public void RandomlyExpand()
    {
        for(int i = 0; i < randomExpansion; i++)
        {
            if(outerRim.Count <= 0){
                break;
            }
            int index = Random.Range(0, outerRim.Count);

            if(outerRim[index].factionIndex < 0)
            {
                outerRim[index].TakeOver(factionIndex);
                
            }
            else
            {
                outerRim.RemoveAt(index);
                i--;
            }
            
        }
    }

    public virtual void Gather(Resources resources)
    {

        int length = resources.amounts.Length;

        for(int i = 0; i < length; i++)
        {
            if(i < resources.amounts.Length)
            {
                this.resources.amounts[i] += resources.amounts[i];
            }
        }

    }


}
