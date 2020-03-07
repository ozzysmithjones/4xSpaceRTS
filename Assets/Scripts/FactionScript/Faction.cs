using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Faction  
{
    //the territory owned by the faction.(some wanderer factions may not own any territory.)
    public List<Star> colonies = new List<Star>();
    public List<Star> territory = new List<Star>();
    public List<Star> outerRim = new List<Star>();

    //the colour of this faction.
    public string factionName;
    public Color flagColor;

    //the different ship and structure types avalible to this faction:
    public BuiltShip[] shipTypes;
    public BuiltStructure[] structureTypes;

    //this integer indicates which index this faction is in the array.
    public int factionIndex;

    

    public Resources resources = new Resources();
    public Resources resourceProduction = new Resources();
    public int expansionCost = 100;


    //randomises this faction.
    public Faction(int index,Color flagColor, string factionName)
    {
        factionIndex = index;
        this.factionName = factionName;
        this.flagColor = flagColor;

        
    }

    void OuterRimChange(Star star, bool addition = false,bool showOuterRim = false)
    {

        if (addition)
        {
            outerRim.Add(star);
            star.SetSelector(showOuterRim, Color.white);
        }
        else
        {
            outerRim.Remove(star);
            star.SetSelector(false, Color.white);
        }
    }

    
    public void AddToTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {

        if (territory.Contains(star))
        {
            return;
        }
        outerRim.Remove(star);

        territory.Add(star);
        if (colony)
        {
            colonies.Add(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();
        for(int i = 0; i < connectedStars.Count; i++)
        {
            if (!territory.Contains(connectedStars[i]) && !outerRim.Contains(connectedStars[i]))
            {
                outerRim.Add(connectedStars[i]);
                if (showOuterRim && connectedStars[i].factionIndex < 0)
                {
                    OuterRimChange(connectedStars[i], true, true);
                }
            }
        }

    }

    public void RemoveFromTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {

        if (!territory.Contains(star))
        {
            return;
        }

        territory.Remove(star);

        if (colony)
        {
            colonies.Remove(star);
        }

        bool shouldBeInOuterRim = star.starConnections.IsConnectedToFaction(factionIndex);
        if (shouldBeInOuterRim != outerRim.Contains(star))
        {
            if (shouldBeInOuterRim)
            {
                OuterRimChange(star, true, showOuterRim);

            }
            else
            {
                OuterRimChange(star, false, showOuterRim);

            }

        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();
        for (int i = 0; i < connectedStars.Count; i++)
        {
            shouldBeInOuterRim = connectedStars[i].starConnections.IsConnectedToFaction(factionIndex);
            if (shouldBeInOuterRim != outerRim.Contains(connectedStars[i]))
            {
                if (shouldBeInOuterRim)
                {
                    OuterRimChange(star, true, showOuterRim);

                }
                else 
                {
                    OuterRimChange(star, false, showOuterRim);

                }

            }
        }
    }


    public void RandomlyExpand(int lowest = 3, int highest = 8)
    {
        int length = Random.Range(lowest, highest);
        for(int i = 0; i < length; i++)
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

        for(int i = 0; i < resources.amounts.Length; i++)
        {
            if(i < this.resources.amounts.Length)
            {
                this.resources.amounts[i] += resources.amounts[i];
            }
        }

    }

    public virtual void ImproveResourceProduction(ResourceType resourceType, int amount)
    {
        resourceProduction.amounts[(int)resourceType] += amount;
    }





}
