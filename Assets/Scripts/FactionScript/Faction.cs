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

    public List<Navigator> fleets = new List<Navigator>();



    //randomises this faction.
    public Faction(int index,Color flagColor, string factionName)
    {
        factionIndex = index;

        this.factionName = factionName;
        this.flagColor = flagColor;

        
    }


    public virtual void Start()
    {

    }

    public virtual void Update(float deltaTime)
    {

    }


    
    public void AddToTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {
        
        outerRim.Remove(star);

        if (territory.Contains(star))
        {
            return;
        }
        territory.Add(star);

        if (colony)
        {
            colonies.Add(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for(int i = 0; i < connectedStars.Count; i++)
        {
            if(!outerRim.Contains(connectedStars[i]) && connectedStars[i].factionIndex != factionIndex)
            {
                outerRim.Add(connectedStars[i]);
            }
        }
       

    }

    public void RemoveFromTerritory(Star star, bool showOuterRim = false, bool colony = false)
    {
        territory.Remove(star);

        if (colony)
        {
            colonies.Remove(star);
        }

        List<Star> connectedStars = star.starConnections.GetConnectedStars();

        for (int i = 0; i < connectedStars.Count; i++)
        {
            if (outerRim.Contains(connectedStars[i]) && !connectedStars[i].starConnections.IsConnectedToFaction(factionIndex))
            {
                outerRim.Remove(connectedStars[i]);
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

    public bool Pay(int cost, ResourceType resourceType = ResourceType.MATERIALS)
    {
        if(resources.amounts[(int)resourceType] >= cost)
        {
            resources.amounts[(int)resourceType] -= cost;
            return true;
        }
        return false;
    }





}
